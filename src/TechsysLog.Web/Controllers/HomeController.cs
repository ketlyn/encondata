using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using TechsysLog.Core.Identity;
using TechsysLog.Identity.Api.Models;
using TechsysLog.Web.Models;
using TechsysLog.Web.Services;

namespace TechsysLog.Web.Controllers
{
    public class HomeController : MainController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAuthService _authenticationService;
        private IAspNetUser _aspNetUser;
        private readonly IOrderService _orderService;
        private readonly IDeliveryService _deliveryService;
        public HomeController(IAuthService authenticationService,
                              IAspNetUser aspNetUser,
                              IOrderService orderService,
                              IDeliveryService deliveryService)
        {
            _authenticationService = authenticationService;
            _aspNetUser = aspNetUser;
            _orderService = orderService;
            _deliveryService = deliveryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        #region autentication

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserLogin userLogin, string returnUrl = null)
        {
            if (!ModelState.IsValid) return View(userLogin);
            var result = await _authenticationService.Login(userLogin);
            if (!ResponseHasErrors(result.ResponseResult))
            {
                await LogIn(result);
                if (string.IsNullOrEmpty(returnUrl))
                    return RedirectToAction(actionName: "TechsysLog", controllerName: "Home");
                return LocalRedirect(returnUrl);
            }
            return View(userLogin);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserRegistration userRegister)
        {
            if (!ModelState.IsValid) return View(userRegister);
            var result = await _authenticationService.Register(userRegister);
            if (ResponseHasErrors(result.ResponseResult)) return View(userRegister);
            await LogIn(result);
            return RedirectToAction(actionName: "TechsysLog", controllerName: "Home");
        }

        [HttpGet]
        [Route("register")]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpGet]
        [Route("new")]
        public async Task<IActionResult> New()
        {
            return View();
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Order

        [HttpPost]
        [Route("new")]
        public async Task<IActionResult> New(AddOrderModel model)
        {
            model = new AddOrderModel
            {
                Number = "1",
                Description = "Order Description",
                Value = 100.00,
                UpdateDate = DateTime.Now,
                Address = new Address("99999999", "Number", 123, "Neighborhood", "City", "State")
            };

            var result = await _orderService.Add(model);
            if (ResponseHasErrors(result)) return View(model);
            return RedirectToAction(actionName: "TechsysLog", controllerName: "Home");
        }


        [HttpGet]
        [Route("TechsysLog")]
        [Authorize]
        public async Task<IActionResult> TechsysLog([FromQuery] int ps = 8, [FromQuery] int page = 1, [FromQuery] string q = null)
        {
            return View(new ListOrderModel
            {
                aspNetUser = _aspNetUser,
                orders = await _orderService.GetAllOrdersPaged(ps, page, q)
            });
        }

        [HttpGet]
        [Route("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _orderService.Delete(id);
            return RedirectToAction(actionName: "TechsysLog", controllerName: "Home");
        }

        #endregion

        #region "Private Methods"
        private async Task LogIn(UserResponseLogin userAnswersLogin)
        {
            var token = GetFormattedToken(userAnswersLogin.AccessToken);
            var claims = new List<Claim>();
            claims.Add(new Claim("JWT", userAnswersLogin.AccessToken));
            claims.AddRange(token.Claims);
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                IsPersistent = true
            };
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
        private static JwtSecurityToken GetFormattedToken(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        }
        #endregion
    }
}
