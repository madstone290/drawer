using Drawer.AidBlazor;
using Drawer.Web.Api.InventoryManagement;
using Drawer.Web.Pages.Inventory.Models;
using Drawer.Web.Services;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.Web.Pages.Inventory
{
    public partial class ItemLocationInventoryHome
    {
        private readonly List<ItemLocationInventoryModel> _modelList = new();
        private readonly ExcelOptions _excelOptions = new ExcelOptionsBuilder()
            .AddColumn(nameof(ItemLocationInventoryModel.ItemName), "아이템")
            .AddColumn(nameof(ItemLocationInventoryModel.LocationName), "위치")
            .AddColumn(nameof(ItemLocationInventoryModel.Quantity), "수량")
            .Build();

        private bool _isTableLoading;
        private bool canCreate = false;
        private bool canRead = false;
        private bool canUpdate = false;
        private bool canDelete = false;

        private string searchText = string.Empty;

        [Inject] public ItemApiClient ItemApiClient { get; set; } = null!;
        [Inject] public LocationApiClient LocationApiClient { get; set; } = null!;
        [Inject] public InventoryApiClient InventoryApiClient { get; set; } = null!;
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

        private bool FilterInventoryDetails(ItemLocationInventoryModel model)
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

            // 모든 아이템/위치에 대한 상세정보 생성
            _modelList.Clear();
            foreach (var item in itemResponse.Data.Items)
            {
                foreach(var location in locationResponse.Data.Locations)
                {
                    var inventoryItemLocation = new ItemLocationInventoryModel()
                    {
                        ItemId = item.Id,
                        ItemName = item.Name,
                        LocationId = location.Id,
                        LocationName = location.Name,
                    };
                    _modelList.Add(inventoryItemLocation);
                }
            }

            // 서버의 수량정보를 적용한다.
            foreach (var inventoryItemLocation in _modelList)
            {
                inventoryItemLocation.Quantity = inventoryResponse.Data.InventoryDetails
                    .Where(x => x.ItemId == inventoryItemLocation.ItemId && x.LocationId ==  inventoryItemLocation.LocationId)
                    .Sum(x => x.Quantity);
            }

            _isTableLoading = false;
        }

        private void Receipt_Click()
        {
            NavManager.NavigateTo(Paths.GoodsReceiptHome);
        }

        private void Issue_Click()
        {
            NavManager.NavigateTo(Paths.GoodsIssueHome);
        }

        private void Transfer_Click()
        {
            NavManager.NavigateTo(Paths.GoodsIssueHome);
        }

        private async Task Download_ClickAsync()
        {
            var fileName = $"위치-{DateTime.Now:yyMMdd-HHmmss}.xlsx";
            await ExcelFileService.Download(fileName, _modelList, _excelOptions);
        }
    }
}
