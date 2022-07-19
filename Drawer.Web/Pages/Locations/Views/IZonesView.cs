using Drawer.Web.Pages.Locations.Models;

namespace Drawer.Web.Pages.Locations.Views
{
    public interface IZonesView
    {
        ZoneTableModel? SelectedZone { get; }

        IList<ZoneTableModel> ZoneList { get; }

        int TotalRowCount { get; set; }

        bool IsTableLoading { get; set; }
    }
}
