using Drawer.WebClient.Pages.Locations.Models;

namespace Drawer.WebClient.Pages.Locations.Views
{
    public interface IWorkPlacesView
    {
        WorkPlaceModel? SelectedWorkPlace { get; }

        IList<WorkPlaceModel> WorkPlaceList { get; }

        int TotalRowCount { get; set; }
    }
}
