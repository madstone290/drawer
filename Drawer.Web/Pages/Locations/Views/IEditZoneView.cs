using Drawer.Web.Pages.Locations.Models;

namespace Drawer.Web.Pages.Locations.Views
{
    public interface IEditZoneView
    {
        ZoneModel Model { get; }

        IList<WorkPlaceModel> WorkPlaceModels { get; }

        void CloseView();
    }
}
