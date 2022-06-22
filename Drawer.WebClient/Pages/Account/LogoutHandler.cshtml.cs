using Drawer.WebClient.Authentication;
using Drawer.WebClient.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Drawer.WebClient.Pages.Account
{
    public class LogoutHandlerModel : PageModel
    {
        private readonly IAuthenticationManager _authenticationManager;

        public LogoutHandlerModel(IAuthenticationManager authenticationManager)
        {
            _authenticationManager = authenticationManager;
        }

        public async Task<IActionResult> OnGetAsync(string? redirectUri)
        {
            await _authenticationManager.LogoutAsync();

            return Redirect(Paths.Account.LogoutCallback.AddQueryParam("redirectUri", redirectUri));
        }
    }
}
