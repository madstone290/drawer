using Drawer.Contract;
using Drawer.Contract.Authentication;
using Drawer.Contract.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace Drawer.WebClient.Authentication
{
    public class AuthenticationManager : IAuthenticationManager
    {
        /// <summary>
        /// Drawer.Api 클라이언트
        /// </summary>
        private readonly HttpClient _httpClient;

        private readonly HttpContext _httpContext;

        public AuthenticationManager(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContext = httpContextAccessor.HttpContext!;
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var loginResponseMessage = await _httpClient.PostAsJsonAsync(ApiRoutes.Account.Login, new LoginRequest(email, password));
            if (!loginResponseMessage.IsSuccessStatusCode)
            {
                var error = await loginResponseMessage.Content.ReadFromJsonAsync<ErrorResponse>();
                if (error!.Code == ErrorCodes.UnconfirmedEmail)
                {
                    return AuthenticationResult.UnconfirmedEmail();
                }
                else
                {
                    return AuthenticationResult.Fail(error.Message);
                }
            }

            // 쿠키인증을 진행한다.
            var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponse>();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim("AccessToken", loginResponse!.AccessToken),
                new Claim("RefreshToken", loginResponse!.RefreshToken)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();

            await _httpContext.SignInAsync(
               CookieAuthenticationDefaults.AuthenticationScheme,
               new ClaimsPrincipal(claimsIdentity),
               authProperties);
            
            return AuthenticationResult.Success();
        }

        public async Task<AuthenticationResult> LogoutAsync()
        {
            await _httpContext.SignOutAsync();

            return AuthenticationResult.Success();
        }
    }
}
