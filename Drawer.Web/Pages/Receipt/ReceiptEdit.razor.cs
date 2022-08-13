using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.Commands;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Web.Api.Inventory;
using Drawer.Web.Pages.Receipt.Models;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.Web.Pages.Receipt
{
    public partial class ReceiptEdit
    {
        private MudForm _form = null!;
        private bool _isFormValid;
        private readonly ReceiptModel _receipt = new();
        private readonly ReceiptModelValidator _validator = new();
        private readonly List<ItemQueryModel> _itemList = new();
        private readonly List<LocationQueryModel> _locationList = new();

        private bool _isLoading;

        public string TitleText
        {
            get
            {
                if (EditMode == EditMode.Add)
                    return "입고 추가";
                else if (EditMode == EditMode.Update)
                    return "입고 수정";
                else
                    return "입고 보기";
            }
        }

        public bool IsViewMode => EditMode == EditMode.View;

        [Parameter] public EditMode EditMode { get; set; }
        [Parameter] public long ReceiptId { get; set; }

        [Inject] public ReceiptApiClient ReceiptApiClient { get; set; } = null!;
        [Inject] public ItemApiClient ItemApiClient { get; set; } = null!;
        [Inject] public LocationApiClient LocationApiClient { get; set; } = null!;


        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;

            var itemTask = ItemApiClient.GetItems()
                .ContinueWith((task) =>
                {
                    var itemResponse = task.Result;
                    if (!Snackbar.CheckFail(itemResponse))
                        return;

                    _itemList.Clear();
                    _itemList.AddRange(itemResponse.Data);
                    _validator.ItemNames = _itemList.Select(x => x.Name).ToList();
                });

            var locationTask = LocationApiClient.GetLocations()
                .ContinueWith((task) =>
                {
                    var locationResponse = task.Result;
                    if (!Snackbar.CheckFail(locationResponse))
                        return;

                    _locationList.Clear();
                    _locationList.AddRange(locationResponse.Data.Where(x => x.IsGroup == false));
                    _validator.LocationNames = _locationList.Select(x => x.Name).ToList();
                });

            var receiptTask = EditMode != EditMode.Update
                ? Task.CompletedTask
                : ReceiptApiClient.GetReceipt(ReceiptId)
                    .ContinueWith((task) =>
                    {
                        var receiptResponse = task.Result;
                        if (!Snackbar.CheckFail(receiptResponse))
                            return;

                        var receiptDto = receiptResponse.Data;
                        if (receiptDto == null)
                        {
                            Snackbar.Add("입고내역을 조회할 수 없습니다", Severity.Error);
                            return;
                        }

                        _receipt.Id = receiptDto.Id;
                        _receipt.ReceiptDate = receiptDto.ReceiptDateTimeLocal.Date;
                        _receipt.ReceiptTime = receiptDto.ReceiptDateTimeLocal.TimeOfDay;
                        _receipt.ItemId = receiptDto.ItemId;
                        _receipt.ItemName = _itemList.First(x => x.Id == receiptDto.ItemId).Name;
                        _receipt.LocationId = receiptDto.LocationId;
                        _receipt.LocationName = _locationList.First(x => x.Id == receiptDto.LocationId).Name;
                        _receipt.Quantity = receiptDto.Quantity;
                        _receipt.Seller = receiptDto.Seller;
                    });

            await Task.WhenAll(itemTask, locationTask, receiptTask);

            _isLoading = false;

        }

        void Back_Click()
        {
            NavManager.NavigateTo(Paths.ReceiptHome);
        }

        async Task Save_Click()
        {
            await _form.Validate();
            if (_isFormValid)
            {
                var receiptDto = new ReceiptCommandModel()
                {
                    ReceiptDateTimeLocal = _receipt.ReceiptDateTime,
                    ItemId = _receipt.ItemId,
                    LocationId = _receipt.LocationId,
                    Quantity = _receipt.Quantity,
                    Seller = _receipt.Seller
                };

                if (EditMode == EditMode.Add)
                {    
                    var response = await ReceiptApiClient.AddReceipt(receiptDto);
                    if (Snackbar.CheckSuccessFail(response))
                    {
                        NavManager.NavigateTo(Paths.ReceiptHome);
                    }
                }
                else if (EditMode == EditMode.Update)
                {
                    var response = await ReceiptApiClient.UpdateReceipt(_receipt.Id, receiptDto);
                    if (Snackbar.CheckSuccessFail(response))
                    {
                        NavManager.NavigateTo(Paths.ReceiptHome);
                    }
                }
            }
        }

    }
}

