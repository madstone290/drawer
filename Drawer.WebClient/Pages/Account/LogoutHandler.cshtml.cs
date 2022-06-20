using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Drawer.WebClient.Pages.Account
{
    public class LogoutHandlerModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync(string redirectUri)
        {
            await HttpContext.SignOutAsync();

            return Redirect(redirectUri);
        }
    }
}
