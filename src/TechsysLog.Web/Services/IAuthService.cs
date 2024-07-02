using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TechsysLog.Core.Communications;
using TechsysLog.Identity.Api.Models;
using TechsysLog.Web.Extensions;

namespace TechsysLog.Web.Services
{
    public interface IAuthService
    {
        Task<UserResponseLogin> Login(UserLogin userLogin);
        Task<UserResponseLogin> Register(UserRegistration userRegister);
    }

    public class AuthService : Service, IAuthService
    {
        private readonly HttpClient _httpClient;
        
        public AuthService(HttpClient httpClient,
            IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.IdentityUrl);
        }
        public async Task<UserResponseLogin> Login(UserLogin userLogin)
        {
            var loginContent = GetContent(userLogin);
            var response = await _httpClient.PostAsync(requestUri: $"/api/identity/authenticate", content: loginContent);
            if (!HandleErrorsResponse(response))
            {
                return new UserResponseLogin
                {
                    ResponseResult = await DeserializeResponseObject<ResponseResult>(response)
                };
            }
            return await DeserializeResponseObject<UserResponseLogin>(response);
        }

        public async Task<UserResponseLogin> Register(UserRegistration userRegister)
        {
            try
            {
                var registroContent = GetContent(userRegister);
                var response = await _httpClient.PostAsync(requestUri: $"/api/identity/register", content: registroContent);
                if (!HandleErrorsResponse(response))
                {
                    return new UserResponseLogin
                    {
                        ResponseResult = await DeserializeResponseObject<ResponseResult>(response)
                    };
                }
                return await DeserializeResponseObject<UserResponseLogin>(response);
            }
            catch (Exception e) {

                throw e;
            }

        }
    }
}
