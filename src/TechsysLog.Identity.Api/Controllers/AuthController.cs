using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TechsysLog.Core.Controllers;
using TechsysLog.Core.Extensions;
using TechsysLog.Core.Identity;
using TechsysLog.Identity.Api.Models;

namespace TechsysLog.Identity.Api.Controllers
{
    [Route("api/identity")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;
        public AuthController(SignInManager<IdentityUser> signInManager,
                             UserManager<IdentityUser> userManager,
                             IOptions<AppSettings> appSettings)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult> Login(UserLogin userLogin)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var result = await _signInManager.PasswordSignInAsync(userName: userLogin.Email, password: userLogin.Password, isPersistent: false, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                return CustomResponse(await GenerateJwt(userLogin.Email));
            }
            AddErrorProcessing(ReturnsReasonBlocking(result));
            return CustomResponse();
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegistration userRegistration)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var user = new IdentityUser
            {
                UserName = userRegistration.Email,
                Email = userRegistration.Email,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(user, userRegistration.Password);
            if (result.Succeeded)
            {
                return CustomResponse(await GenerateJwt(userRegistration.Email));
            }
            result.Errors.ForEach(x => AddErrorProcessing(x.Description));
            return CustomResponse();
        }

        #region "Private"
        private string ReturnsReasonBlocking(Microsoft.AspNetCore.Identity.SignInResult result)
        {
            if (result.IsLockedOut) return "User is blocked for invalid attempts";
            if (result.IsNotAllowed) return "User is not authorized";
            if (result.RequiresTwoFactor) return "Requires two-step authentication";
            return "Unrecognized username or password";
        }
        private async Task<UserResponseLogin> GenerateJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);
            var identityClaims = await GetUserClaims(claims, user);
            var encodedToken = EncodeToken(identityClaims);
            return GetResponseToken(encodedToken, user, claims);
        }
        private UserResponseLogin GetResponseToken(string encodedToken, IdentityUser user, IEnumerable<Claim> claims)
        {
            return new UserResponseLogin
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpirationHours).TotalSeconds,
                UsuarioToken = new UserToken
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(c => new UserClaim { Type = c.Type, Value = c.Value })
                }
            };
        }
        private async Task<ClaimsIdentity> GetUserClaims(ICollection<Claim> claims, IdentityUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));//quando ele foi emitido
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(type: "role", value: userRole));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            return identityClaims;
        }
        private string EncodeToken(ClaimsIdentity identityClaims)
        {
            
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = _appSettings.Issuer,
                    Audience = _appSettings.ValidOn,
                    Subject = identityClaims,
                    Expires = DateTime.UtcNow.AddHours(_appSettings.ExpirationHours),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret)),
                    SecurityAlgorithms.HmacSha256Signature)
                });
                return tokenHandler.WriteToken(token);
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
        #endregion
    }
}
