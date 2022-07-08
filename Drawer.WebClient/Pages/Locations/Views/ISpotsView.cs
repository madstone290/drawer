using Drawer.WebClient.Pages.Locations.Models;

namespace Drawer.WebClient.Pages.Locations.Views
{
    public interface ISpotsView
    {
        SpotTableModel? SelectedSpot { get; }

        IList<SpotTableModel> SpotList { get; }

        int TotalRowCount { get; set; }

        bool IsTableLoading { get; set; }
    }
}
