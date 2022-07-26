using Drawer.AidBlazor;
using Drawer.Web.Pages.Items.Models;
using Drawer.Web.Pages.Items.Presenters;
using Drawer.Web.Pages.Items.Views;
using Microsoft.AspNetCore.Components;

namespace Drawer.Web.Pages.Items
{
    public partial class ItemTable : IItemView
    {
        private AidTable<ItemTableModel> table = null!;

        private bool canCreate = false;
        private bool canRead = false;
        private bool canUpdate = false;
        private bool canDelete = false;

        private string searchText = string.Empty;

        [Inject]
        public ItemPresenter Presenter { get; set; } = null!;

        #region IItemView

        public ItemTableModel? SelectedItem => table.FocusedItem;

        public IList<ItemTableModel> ItemList { get; private set; } = new List<ItemTableModel>();

        public int TotalRowCount { get; set; }

        public bool IsTableLoading { get; set; }

        #endregion

        protected override async Task OnInitializedAsync()
        {
            canCreate = true;
            canRead = true;
            canUpdate = true;
            canDelete = true;

            Presenter.View = this;
            await Presenter.LoadItemListAsync();
        }

        private bool Filter(ItemTableModel item)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return true;
            if (item == null)
                return false;

            return item.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) || 
                item.Code.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                item.Number.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                item.Sku.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                item.QuantityUnit.Contains(searchText, StringComparison.OrdinalIgnoreCase);
        }

        private async Task Load_Click()
        {
            await Presenter.LoadItemListAsync();
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

        private async Task BatchAdd_Click()
        {
            await Presenter.ShowBatchEditDialog();
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
