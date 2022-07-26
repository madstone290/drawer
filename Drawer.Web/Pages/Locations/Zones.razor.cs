using Drawer.AidBlazor;
using Drawer.Web.Pages.Locations.Models;
using Drawer.Web.Pages.Locations.Presenters;
using Drawer.Web.Pages.Locations.Views;
using Microsoft.AspNetCore.Components;

namespace Drawer.Web.Pages.Locations
{
    public partial class Zones : IZonesView
    {
        private AidTable<ZoneTableModel> table = null!;

        private bool canCreate = false;
        private bool canRead = false;
        private bool canUpdate = false;
        private bool canDelete = false;

        private string searchText = string.Empty;

        public IList<ZoneTableModel> ZoneList { get; private set; } = new List<ZoneTableModel>();

        [Inject]
        public ZonesPresenter Presenter { get; set; } = null!;

        public ZoneTableModel? SelectedZone => table.FocusedItem;

        public int TotalRowCount { get; set; }
        public bool IsTableLoading { get; set; }

        protected override async Task OnInitializedAsync()
        {
            canCreate = true;
            canRead = true;
            canUpdate = true;
            canDelete = true;

            Presenter.View = this;
            await Presenter.LoadZonesAsync();
        }

        private bool FilterZones(ZoneTableModel model)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return true;
            if (model == null)
                return false;

            return model.Note.Contains(searchText, StringComparison.OrdinalIgnoreCase) || 
                model.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) || 
                model.WorkPlaceName.Contains(searchText, StringComparison.OrdinalIgnoreCase);
        }

        private async Task Load_Click()
        {
            await Presenter.LoadZonesAsync();
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
