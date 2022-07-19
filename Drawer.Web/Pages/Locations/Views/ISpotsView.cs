using Drawer.Web.Pages.Locations.Models;

namespace Drawer.Web.Pages.Locations.Views
{
    public interface ISpotsView
    {
        SpotTableModel? SelectedSpot { get; }

        IList<SpotTableModel> SpotList { get; }

        int TotalRowCount { get; set; }

        bool IsTableLoading { get; set; }
    }
}
