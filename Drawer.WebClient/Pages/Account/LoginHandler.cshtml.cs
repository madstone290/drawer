using Drawer.Contract.Authentication;
using Drawer.Contract.Common;
using Drawer.WebClient.Authentication;
using Drawer.WebClient.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Drawer.WebClient.Pages.Account
{
    public class LoginHandlerModel : PageModel
    {
        private readonly IAuthenticationManager _authenticationManager;

        public LoginHandlerModel(IAuthenticationManager authenticationManager)
        {
            _authenticationManager = authenticationManager;
        }

        public async Task<IActionResult> OnGetAsync(string email, string password)
        {
<<<<<<< Updated upstream
			var loginResponseMessage = await _httpClient.PostAsJsonAsync("/api/account/login", new LoginRequest(email, password));
			if (!loginResponseMessage.IsSuccessStatusCode)
			{
				var error = await loginResponseMessage.Content.ReadFromJsonAsync<ErrorResponse>();
				if(error!.Code == ErrorCodes.UnconfirmedEmail)
=======
            var loginResult = await _authenticationManager.LoginAsync(email, password);
            if (loginResult.IsSuccessful)
            {
                return Redirect(Paths.Base);
            }
            else if (loginResult.UnconfirmedEmail)
            {
                return Redirect(Paths.Account.ConfirmEmail.AddQueryParam("email", email));
            }
            else
            {
                return Redirect(Paths.Account.Login.AddQueryParam("error", loginResult!.Message));
            }
            
            var loginResponseMessage = await _httpClient.PostAsJsonAsync("/api/account/login", new LoginRequest(email, password));
            if (!loginResponseMessage.IsSuccessStatusCode)
            {
                var error = await loginResponseMessage.Content.ReadFromJsonAsync<ErrorResponse>();
                if (error!.Code == ErrorCodes.NotConfirmedEmail)
>>>>>>> Stashed changes
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
