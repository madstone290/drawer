using Drawer.Contract.Inventory;
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
        private readonly List<GetItemsResponse.Item> _itemList = new();
        private readonly List<GetLocationsResponse.Location> _locationList = new();

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

            var itemResponse = await ItemApiClient.GetItems();
            var locationResponse = await LocationApiClient.GetLocations();
            if (!Snackbar.CheckFail(itemResponse, locationResponse))
                return;

            _itemList.Clear();
            _itemList.AddRange(itemResponse.Data.Items);

            _locationList.Clear();
            _locationList.AddRange(locationResponse.Data.Locations.Where(x => x.IsGroup == false));

            _validator.ItemNames = _itemList.Select(x => x.Name).ToList();
            _validator.LocationNames = _locationList.Select(x => x.Name).ToList();

            if (EditMode == EditMode.Update)
            {
                var receiptResponse = await ReceiptApiClient.GetReceipt(ReceiptId);
                if (!Snackbar.CheckFail(receiptResponse))
                    return;

                var receiptDto = receiptResponse.Data.Receipt;

                _receipt.Id = receiptDto.Id;
                _receipt.ReceiptDate = receiptDto.ReceiptDateTime.Date;
                _receipt.ReceiptTime = receiptDto.ReceiptDateTime.TimeOfDay;
                _receipt.ItemId = receiptDto.ItemId;
                _receipt.ItemName = _itemList.First(x => x.Id == receiptDto.ItemId).Name;
                _receipt.LocationId = receiptDto.LocationId;
                _receipt.LocationName = _locationList.First(x => x.Id == receiptDto.LocationId).Name;
                _receipt.Quantity = receiptDto.Quantity;
                _receipt.Seller = receiptDto.Seller;
            }


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
                if (EditMode == EditMode.Add)
                {
                    var content = new CreateReceiptRequest(_receipt.ReceiptDateTime, _receipt.ItemId, _receipt.LocationId,
                        _receipt.Quantity, _receipt.Seller);
                    var response = await ReceiptApiClient.AddReceipt(content);
                    if (Snackbar.CheckSuccessFail(response))
                    {
                        NavManager.NavigateTo(Paths.ReceiptHome);
                    }
                }
                else if (EditMode == EditMode.Update)
                {
                    var content = new UpdateReceiptRequest(_receipt.ReceiptDateTime, _receipt.ItemId, _receipt.LocationId,
                        _receipt.Quantity, _receipt.Seller);
                    var response = await ReceiptApiClient.UpdateReceipt(_receipt.Id, content);
                    if (Snackbar.CheckSuccessFail(response))
                    {
                        NavManager.NavigateTo(Paths.ReceiptHome);
                    }
                }
            }
        }

    }
}

