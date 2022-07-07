using Drawer.WebClient.Pages.Organization.Models;

namespace Drawer.WebClient.Pages.Organization.Views
{
    public interface IEditCompanyView 
    {
        EditCompanyModel Model { get; set; }

        void CloseView();

    }
}
