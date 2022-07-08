using Drawer.WebClient.Pages.Locations.Models;

namespace Drawer.WebClient.Pages.Locations.Views
{
    public interface IEditSpotView
    {
        SpotModel SpotModel { get; }

        IList<ZoneModel> ZoneModels { get; }

        void CloseView();
    }
}
