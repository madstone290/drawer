using Drawer.Web.Authentication;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Drawer.Web.Pages.Account
{
    public class LoginHandlerModel : PageModel
    {
        private readonly IAuthenticationManager _authenticationManager;

        public LoginHandlerModel(IAuthenticationManager authenticationManager)
        {
            _authenticationManager = authenticationManager;
        }

        public async Task<IActionResult> OnGetAsync(string email, string password, string redirectUri)
        {
            var loginResult = await _authenticationManager.LoginAsync(email, password);
            if (loginResult.IsSuccessful)
            {
                return Redirect(Paths.Account.LoginCallback.AddQuery("redirectUri", redirectUri));
            }
            else if (loginResult.IsUnconfirmedEmail)
            {
                return Redirect(Paths.Account.ConfirmEmail.AddQuery("email", email));
            }
            else
            {
                return Redirect(Paths.Account.Login.AddQuery("error", loginResult!.ErrorMessage));
            }
        }



    }
}
