using Drawer.AidBlazor;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Web.Api.Inventory;
using Drawer.Web.Pages.InventoryStatus.Models;
using Drawer.Web.Services;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.Web.Pages.InventoryStatus
{
    public partial class LocationItemInventoryHome
    {

        private readonly List<InventoryItemModel> _modelList = new();
        private readonly ExcelOptions _excelOptions = new ExcelOptionsBuilder()
            .AddColumn(nameof(InventoryItemModel.ItemName), "아이템")
            .AddColumn(nameof(InventoryItemModel.LocationName), "위치")
            .AddColumn(nameof(InventoryItemModel.Quantity), "수량")
            .Build();

        private readonly List<LocationQueryModel> _locations = new();
        private readonly List<ItemQueryModel> _items = new();
        private readonly List<InventoryItemQueryModel> _inventoryItems = new();

        private bool _isTableLoading;
        private bool canCreate = false;
        private bool canRead = false;
        private bool canUpdate = false;
        private bool canDelete = false;

        private string searchText = string.Empty;

        [Inject] public ItemApiClient ItemApiClient { get; set; } = null!;
        [Inject] public LocationApiClient LocationApiClient { get; set; } = null!;
        [Inject] public InventoryItemApiClient InventoryApiClient { get; set; } = null!;
        [Inject] public IDialogService DialogService { get; set; } = null!;
        [Inject] public IExcelFileService ExcelFileService { get; set; } = null!;

        public int TotalRowCount => _modelList.Count;


        protected override async Task OnInitializedAsync()
        {
            canCreate = true;
            canRead = true;
            canUpdate = true;
            canDelete = true;

            await Load_Click();
        }

        private bool FilterInventoryDetails(InventoryItemModel model)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return true;
            if (model == null)
                return false;

            return model.ItemName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true ||
                model.LocationName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true ||
                model.Quantity.ToString().Contains(searchText, StringComparison.OrdinalIgnoreCase) == true;


        }

        private async Task Load_Click()
        {
            _isTableLoading = true;

            var itemResponse = await ItemApiClient.GetItems();
            var locationResponse = await LocationApiClient.GetLocations();
            var inventoryResponse = await InventoryApiClient.GetInventoryDetails();
            if (!Snackbar.CheckFail(itemResponse, inventoryResponse))
            {
                _isTableLoading = false;
                return;
            }

            _items.Clear();
            _items.AddRange(itemResponse.Data);

            _locations.Clear();
            _locations.AddRange(locationResponse.Data);

            _inventoryItems.Clear();
            _inventoryItems.AddRange(inventoryResponse.Data);

            _isTableLoading = false;
        }

        private void Receipt_Click()
        {
            NavManager.NavigateTo(Paths.ReceiptHome);
        }

        private void Issue_Click()
        {
            NavManager.NavigateTo(Paths.IssueHome);
        }

        private void Transfer_Click()
        {
            NavManager.NavigateTo(Paths.IssueHome);
        }

        private async Task Download_ClickAsync()
        {
            var fileName = $"위치-{DateTime.Now:yyMMdd-HHmmss}.xlsx";
            await ExcelFileService.Download(fileName, _modelList, _excelOptions);
        }

        private void Field_KeyChanged(long key)
        {
            DisplayLocationInventory(key);
        }

        private void DisplayLocationInventory(long locationId)
        {
        }
        
    }
}
