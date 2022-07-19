using Drawer.Web.Pages.Locations.Models;

namespace Drawer.Web.Pages.Locations.Views
{
    public interface IEditSpotView
    {
        SpotModel SpotModel { get; }

        IList<ZoneModel> ZoneModels { get; }

        void CloseView();
    }
}
