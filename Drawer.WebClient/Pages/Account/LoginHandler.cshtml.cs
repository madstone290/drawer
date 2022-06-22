using Drawer.WebClient.Authentication;
using Drawer.WebClient.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
            var loginResult = await _authenticationManager.LoginAsync(email, password);
            if (loginResult.IsSuccessful)
            {
                return Redirect(Paths.Base);
            }
            else if (loginResult.IsUnconfirmedEmail)
            {
                return Redirect(Paths.Account.ConfirmEmail.AddQueryParam("email", email));
            }
            else
            {
                return Redirect(Paths.Account.Login.AddQueryParam("error", loginResult!.ErrorMessage));
            }
        }



    }
}
