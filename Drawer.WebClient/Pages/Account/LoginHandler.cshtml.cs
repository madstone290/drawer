using Drawer.Contract.Authentication;
using Drawer.Contract.Common;
using Drawer.WebClient.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Drawer.WebClient.Pages.Account
{
    public class LoginHandlerModel : PageModel
    {
		private readonly HttpClient _httpClient;

        public LoginHandlerModel(IHttpClientFactory httpClientFactory)
        {
			_httpClient = httpClientFactory.CreateClient(Constants.HttpClient.DrawerApi);
        }

        public async Task<IActionResult> OnGetAsync(string email, string password)
        {
			var loginResponseMessage = await _httpClient.PostAsJsonAsync("/api/account/login", new LoginRequest(email, password));
			if (!loginResponseMessage.IsSuccessStatusCode)
			{
				var error = await loginResponseMessage.Content.ReadFromJsonAsync<ErrorResponse>();
				if(error!.Code == ErrorCodes.NotConfirmedEmail)
                {
					return Redirect(Paths.Account.ConfirmEmail.AddQueryParam("email", email));
                }
                else
                {
					return Redirect(Paths.Account.Login.AddQueryParam("error", error!.Message));
				}
			}
			
			var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponse>();

			var claims = new List<Claim>();
			claims.Add(new Claim(ClaimTypes.Email, email));
			claims.Add(new Claim("AccessToken", loginResponse!.AccessToken));
			claims.Add(new Claim("RefreshToken", loginResponse!.RefreshToken));

			var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			var authProperties = new AuthenticationProperties();

			await HttpContext.SignInAsync(
			   CookieAuthenticationDefaults.AuthenticationScheme,
			   new ClaimsPrincipal(claimsIdentity),
			   authProperties);

			return Redirect(Paths.Base);
		}



	}
}
