using Drawer.WebClient.Components;
using Drawer.WebClient.Pages.Locations.Models;
using Drawer.WebClient.Pages.Locations.Presenters;
using Drawer.WebClient.Pages.Locations.Views;
using Microsoft.AspNetCore.Components;

namespace Drawer.WebClient.Pages.Locations
{
    public partial class Spots : ISpotsView
    {
        private AMudTable<SpotTableModel> table = null!;

        private bool canCreate = false;
        private bool canRead = false;
        private bool canUpdate = false;
        private bool canDelete = false;

        private string searchText = string.Empty;

        public IList<SpotTableModel> SpotList { get; private set; } = new List<SpotTableModel>();

        [Inject]
        public SpotsPresenter Presenter { get; set; } = null!;

        public SpotTableModel? SelectedSpot => table.FocusedItem;

        public int TotalRowCount { get; set; }
        public bool IsTableLoading { get; set; }

        protected override async Task OnInitializedAsync()
        {
            canCreate = true;
            canRead = true;
            canUpdate = true;
            canDelete = true;

            Presenter.View = this;
            await Presenter.LoadSpotsAsync();
        }

        private bool FilterSpots(SpotTableModel model)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return true;
            if (model == null)
                return false;

            return model.Note.Contains(searchText, StringComparison.OrdinalIgnoreCase) || 
                model.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                model.WorkPlaceName.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                model.ZoneName.Contains(searchText, StringComparison.OrdinalIgnoreCase);
        }

        private async Task Load_Click()
        {
            await Presenter.LoadSpotsAsync();
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
