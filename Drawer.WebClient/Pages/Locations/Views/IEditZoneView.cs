using Drawer.WebClient.Pages.Locations.Models;

namespace Drawer.WebClient.Pages.Locations.Views
{
    public interface IEditZoneView
    {
        ZoneModel Model { get; }

        IList<WorkPlaceModel> WorkPlaceModels { get; }

        void CloseView();
    }
}
