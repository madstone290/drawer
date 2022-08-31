using Drawer.AidBlazor;
using Drawer.Web.Api.Inventory;
using Drawer.Web.Pages.InventoryStatus.Models;
using Drawer.Web.Services;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.Web.Pages.InventoryStatus
{
    public partial class ItemLocationInventoryHome
    {
        private readonly ExcelOptions _excelOptions = new ExcelOptionsBuilder()
            .AddColumn(nameof(InventoryItemModel.ItemName), "아이템")
            .AddColumn(nameof(InventoryItemModel.LocationName), "위치")
            .AddColumn(nameof(InventoryItemModel.Quantity), "수량")
            .Build();

        /// <summary>
        /// 전체 재고 정보
        /// </summary>
        private readonly List<InventoryItemModel> _modelList = new();

        /// <summary>
        /// 화면에 표시할 재고 정보
        /// </summary>
        private readonly List<InventoryItemModel> _displayModelList = new();


        private AidTable<InventoryItemModel> _table = null!;

        private bool _hideZeroQuantity;
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

        public int TotalRowCount => _displayModelList.Count;

        public bool HideZeroQuantity
        {
            get => _hideZeroQuantity;
            set
            {
                if (_hideZeroQuantity == value)
                    return;
                _hideZeroQuantity = value;

                RefreshDisplayList();
            }
        }


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

            // 모든 아이템/위치에 대한 상세정보 생성
            _modelList.Clear();
            foreach (var item in itemResponse.Data)
            {
                foreach (var location in locationResponse.Data)
                {
                    if (location.IsGroup)
                        continue;

                    var quantity = inventoryResponse.Data
                        .Where(x => x.ItemId == item.Id && x.LocationId == location.Id)
                        .Sum(x => x.Quantity);
                    var inventoryItemLocation = new InventoryItemModel()
                    {
                        ItemId = item.Id,
                        ItemName = item.Name,
                        LocationId = location.Id,
                        LocationName = location.Name,
                        Quantity = quantity
                    };
                    _modelList.Add(inventoryItemLocation);
                }
            }

            RefreshDisplayList();

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

        /// <summary>
        /// 화면에 표시할 리스트를 갱신한다.
        /// </summary>
        void RefreshDisplayList()
        {
            _displayModelList.Clear();
            if (HideZeroQuantity)
                _displayModelList.AddRange(_modelList.Where(x => 0 < x.Quantity));
            else
                _displayModelList.AddRange(_modelList);
        }
    }
}
