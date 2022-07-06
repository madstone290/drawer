using Drawer.WebClient.Components;
using Drawer.WebClient.Pages.Locations.Models;
using Drawer.WebClient.Pages.Locations.Presenters;
using Drawer.WebClient.Pages.Locations.Views;
using Microsoft.AspNetCore.Components;

namespace Drawer.WebClient.Pages.Locations
{
    public partial class WorkPlaces : IWorkPlacesView
    {
        private AMudTable<WorkPlaceModel> table = null!;

        private bool canCreate = false;
        private bool canRead = false;
        private bool canUpdate = false;
        private bool canDelete = false;

        private string searchText = string.Empty;

        public IList<WorkPlaceModel> WorkPlaceList { get; private set; } = new List<WorkPlaceModel>();

        [Inject]
        public WorkPlacesPresenter Presenter { get; set; } = null!;

        public WorkPlaceModel? SelectedWorkPlace => table.FocusedItem;

        public int TotalRowCount { get; set; }

        protected override async Task OnInitializedAsync()
        {
            canCreate = true;
            canRead = true;
            canUpdate = true;
            canDelete = true;

            Presenter.View = this;
            await Presenter.LoadWorkPlacesAsync();
        }

        private bool FilterWorkPlaces(WorkPlaceModel model)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return true;
            if (model == null)
                return false;

            return model.Note.Contains(searchText)
            || model.Name.Contains(searchText);
        }

        private async Task Load_Click()
        {
            await Presenter.LoadWorkPlacesAsync();
        }

        private async Task Add_Click()
        {
            await Presenter.ShowAddDialog();
        }

        private async Task Update_Click()
        {
            await Presenter.ShowUpdateDialog();
        }

        private async Task Delete_Click()
        {
            await Presenter.ShowDeleteDialog();
        }

        private void Upload_Click()
        {
            Presenter.UploadExcel();
        }

        private void Download_Click()
        {
            Presenter.DownloadExcel();
        }
    }
}
