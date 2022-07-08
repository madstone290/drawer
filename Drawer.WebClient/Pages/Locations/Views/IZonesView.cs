using Drawer.WebClient.Pages.Locations.Models;

namespace Drawer.WebClient.Pages.Locations.Views
{
    public interface IZonesView
    {
        ZoneTableModel? SelectedZone { get; }

        IList<ZoneTableModel> ZoneList { get; }

        int TotalRowCount { get; set; }

        bool IsTableLoading { get; set; }
    }
}
