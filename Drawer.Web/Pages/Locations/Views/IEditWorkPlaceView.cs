using Drawer.Web.Pages.Locations.Models;

namespace Drawer.Web.Pages.Locations.Views
{
    public interface IEditWorkPlaceView
    {
        WorkPlaceModel Model { get; }

        void CloseView();
    }
}
