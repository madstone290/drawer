using Drawer.WebClient.Utils;
using Microsoft.AspNetCore.Components;

namespace Drawer.WebClient.Pages.Account
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
