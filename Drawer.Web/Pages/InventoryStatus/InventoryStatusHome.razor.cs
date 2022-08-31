using Drawer.AidBlazor;
using Drawer.Web.Api.Inventory;
using Drawer.Web.Pages.InventoryStatus.Models;
using Drawer.Web.Services;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.Web.Pages.InventoryStatus
{
    public partial class InventoryStatusHome
    {
        private AidTable<InventorySumItemModel> table = null!;
        private readonly List<InventorySumItemModel> _inventoryItems = new();
        private readonly ExcelOptions _excelOptions = new ExcelOptionsBuilder()
            .AddColumn(nameof(InventorySumItemModel.ItemName), "아이템")
            .AddColumn(nameof(InventorySumItemModel.Quantity), "수량")
            .Build();

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

        public int TotalRowCount => _inventoryItems.Count;


        protected override async Task OnInitializedAsync()
        {
            canCreate = true;
            canRead = true;
            canUpdate = true;
            canDelete = true;

            await Load_Click();
        }

        private bool FilterInventoryDetails(InventorySumItemModel model)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return true;
            if (model == null)
                return false;

            return model.ItemName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true ||
                model.Quantity.ToString().Contains(searchText, StringComparison.OrdinalIgnoreCase) == true;
        }

        private async Task Load_Click()
        {
            _isTableLoading = true;

            var itemTask = ItemApiClient.GetItems();
            var inventoryTask= InventoryApiClient.GetInventoryDetails();
            await Task.WhenAll(itemTask, inventoryTask);

            var itemResponse = itemTask.Result;
            var inventoryResponse = inventoryTask.Result;

            if (!Snackbar.CheckFail(itemResponse, inventoryResponse))
            {
                _isTableLoading = false;
                return;
            }

            // 모든 아이템에 대한 상세정보 생성
            _inventoryItems.Clear();
            foreach (var item in itemResponse.Data)
            {
                var inventoryDetail = new InventorySumItemModel()
                {
                    ItemId = item.Id,
                    ItemName = item.Name,
                };
                _inventoryItems.Add(inventoryDetail);
            }

            // 서버의 수량정보를 적용한다.
            foreach (var inventoryDetail in _inventoryItems)
            {
                inventoryDetail.Quantity = inventoryResponse.Data
                    .Where(x => x.ItemId == inventoryDetail.ItemId)
                    .Sum(x => x.Quantity);
            }

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
            var fileName = $"재고-{DateTime.Now:yyMMdd-HHmmss}.xlsx";
            await ExcelFileService.Download(fileName, _inventoryItems, _excelOptions);
        }
    }
}
