using Drawer.Web.Pages.Organization.Models;

namespace Drawer.Web.Pages.Organization.Views
{
    public interface IEditCompanyView 
    {
        EditCompanyModel Model { get; set; }

        void CloseView();

    }
}
