using Drawer.AidBlazor;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Web.Api.Inventory;
using Drawer.Web.Pages.Receipt.Models;
using Drawer.Web.Services;
using Drawer.Web.Shared;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.Web.Pages.Receipt
{
    public partial class ReceiptHome
    {
        private AidTable<ReceiptTableModel> table = null!;
        private readonly List<ReceiptTableModel> _receiptList = new();
        private readonly List<ItemQueryModel> _itemList = new();
        private readonly List<LocationQueryModel> _locationList = new();

        private readonly ExcelOptions _excelOptions = new ExcelOptionsBuilder()
            .AddColumn(nameof(ReceiptTableModel.TransactionNumber), "입고번호")
            .AddColumn(nameof(ReceiptTableModel.ReceiptDateString), "입고일자")
            .AddColumn(nameof(ReceiptTableModel.ReceiptTimeString), "입고시간")
            .AddColumn(nameof(ReceiptTableModel.ItemName), "아이템")
            .AddColumn(nameof(ReceiptTableModel.LocationName), "위치")
            .AddColumn(nameof(ReceiptTableModel.QuantityString), "수량")
            .AddColumn(nameof(ReceiptTableModel.Seller), "판매자")
            .AddColumn(nameof(ReceiptTableModel.Note), "비고")
            .Build();

        private bool _isLoading;
        private bool canCreate = false;
        private bool canRead = false;
        private bool canUpdate = false;
        private bool canDelete = false;

        private string searchText = string.Empty;

        /// <summary>
        /// 조회 시작일
        /// </summary>
        private DateTime? _receiptDateFrom = DateTime.Today;
        /// <summary>
        /// 조회 종료일
        /// </summary>
        private DateTime? _receiptDateTo = DateTime.Today;

        [Inject] public ReceiptApiClient ReceiptApiClient { get; set; } = null!;
        [Inject] public ItemApiClient ItemApiClient { get; set; } = null!;
        [Inject] public LocationApiClient LocationApiClient { get; set; } = null!;
        [Inject] public IDialogService DialogService { get; set; } = null!;
        [Inject] public IExcelFileService ExcelFileService { get; set; } = null!;

        public ReceiptTableModel? SelectedReceipt => table.FocusedItem;
        public int TotalRowCount => _receiptList.Count;


        protected override async Task OnInitializedAsync()
        {
            canCreate = true;
            canRead = true;
            canUpdate = true;
            canDelete = true;

            await Load_Click();
        }

        private bool FilterReceipts(ReceiptTableModel model)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return true;
            if (model == null)
                return false;

            return model.TransactionNumber?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true ||
                model.ReceiptDateString?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true ||
                model.ReceiptTimeString?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true ||
                model.ItemName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true ||
                model.LocationName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true ||
                model.QuantityString?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true ||
                model.Seller?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true;
        }

        private async Task Load_Click()
        {
            if (!_receiptDateFrom.HasValue || !_receiptDateTo.HasValue)
            {
                Snackbar.Add("조회 기간을 선택하세요");
                return;
            }

            _isLoading = true;

            var receiptTask = ReceiptApiClient.GetReceipts(_receiptDateFrom.Value, _receiptDateTo.Value)
                .ContinueWith((task) =>
                {
                    var receiptResponse = task.Result;
                    if (!Snackbar.CheckFail(receiptResponse))
                        return;

                    foreach (var receiptDto in receiptResponse.Data)
                    {
                        var receipt = new ReceiptTableModel()
                        {
                            Id = receiptDto.Id,
                            TransactionNumber = receiptDto.TransactionNumber,
                            ReceiptDateString = receiptDto.ReceiptDateTimeLocal.Date.ToString("yyyy-MM-dd"),
                            ReceiptTimeString = receiptDto.ReceiptDateTimeLocal.TimeOfDay.ToString(@"hh\:mm"),
                            ItemId = receiptDto.ItemId,
                            LocationId = receiptDto.LocationId,
                            QuantityString = receiptDto.Quantity.ToString(),
                            Seller = receiptDto.Seller,
                            Note = receiptDto.Note
                        };
                        _receiptList.Add(receipt);
                    }
                });


            var itemTask = ItemApiClient.GetItems()
                .ContinueWith((task) =>
                {
                    var itemResponse = task.Result;
                    if (Snackbar.CheckFail(itemResponse))
                    {
                        _itemList.Clear();
                        _itemList.AddRange(itemResponse.Data);
                    }
                });

            var locationTask = LocationApiClient.GetLocations()
                .ContinueWith((task) =>
                {
                    var locationResponse = task.Result;
                    if (Snackbar.CheckFail(locationResponse))
                    {
                        _locationList.Clear();
                        _locationList.AddRange(locationResponse.Data.Where(x => x.IsGroup == false));
                    }
                });

            await Task.WhenAll(receiptTask, itemTask, locationTask);

            foreach (var receipt in _receiptList)
            {
                receipt.ItemName = _itemList.First(x => x.Id == receipt.ItemId).Name;
                receipt.LocationName = _locationList.First(x => x.Id == receipt.LocationId).Name;
            }

            _isLoading = false;
        }

        private void Add_Click()
        {
            NavManager.NavigateTo(Paths.ReceiptAdd);
        }

        private void Update_Click()
        {

            if (SelectedReceipt == null)
            {
                Snackbar.Add("입고를 먼저 선택하세요", Severity.Normal);
                return;
            }
            NavManager.NavigateTo(Paths.ReceiptUpdate.Replace("{id}", $"{SelectedReceipt.Id}"));
        }

        private async Task Delete_Click()
        {
            if (SelectedReceipt == null)
            {
                Snackbar.Add("입고를 먼저 선택하세요", Severity.Normal);
                return;
            }

            var selectedReceipt = SelectedReceipt;

            var dialogOptions = new DialogOptions()
            {
                MaxWidth = MaxWidth.Small,
            };
            var dialogParameters = new DialogParameters
            {
                { nameof(DeleteDialog.Message), $"{selectedReceipt.TransactionNumber} 입고를 삭제하시겠습니까?" }
            };
            var dialog = DialogService.Show<DeleteDialog>(null, options: dialogOptions, parameters: dialogParameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await ReceiptApiClient.RemoveReceipt(selectedReceipt.Id);
                if (Snackbar.CheckSuccessFail(response))
                {
                    _receiptList.Remove(selectedReceipt);
                }
            }
        }

        private void BatchEdit_Click()
        {
            NavManager.NavigateTo(Paths.ReceiptBatchAdd);
        }

        private async Task Download_ClickAsync()
        {
            var fileName = $"입고-{DateTime.Now:yyMMdd-HHmmss}.xlsx";
            await ExcelFileService.Download(fileName, _receiptList, _excelOptions);
        }
    }
}
