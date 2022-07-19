using Drawer.Web.Pages.Locations.Models;

namespace Drawer.Web.Pages.Locations.Views
{
    public interface IWorkPlacesView
    {
        WorkPlaceModel? SelectedWorkPlace { get; }

        IList<WorkPlaceModel> WorkPlaceList { get; }

        int TotalRowCount { get; set; }

        bool IsTableLoading { get; set; }
    }
}
