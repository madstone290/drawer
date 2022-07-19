using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;

namespace Drawer.Web.Pages.Account
{
    public partial class Logout
    {
        [Inject] NavigationManager NavigationManager { get; set; } = null!;

        void Submit()
        {
            NavigationManager.NavigateTo(Paths.Account.LogoutHandler, true);
        }
    }
}
