using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Web.Api.Inventory;
using Drawer.Web.Pages.Item.Models;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.Web.Pages.Item
{
    public partial class ItemEdit
    {
        private MudForm _form = null!;
        private bool _isFormValid;
        private readonly ItemModel _item = new();
        private readonly ItemModelValidator _validator = new();

        public string TitleText
        {
            get
            {
                if (EditMode == EditMode.Add)
                    return "아이템 추가";
                else if (EditMode == EditMode.Update)
                    return "아이템 수정";
                else
                    return "아이템 보기";
            }
        }

        public bool IsViewMode => EditMode == EditMode.View;

        [Parameter] public EditMode EditMode { get; set; }
        [Parameter] public long? ItemId { get; set; }

        [Inject] public ItemApiClient ApiClient { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            if (ItemId.HasValue)
            {
                var response = await ApiClient.GetItem(ItemId.Value);
                if (Snackbar.CheckFail(response))
                {
                    _item.Id = response.Data.Id;
                    _item.Name = response.Data.Name;
                    _item.Code = response.Data.Code ?? string.Empty;
                    _item.Number = response.Data.Number ?? string.Empty;
                    _item.Sku = response.Data.Sku ?? string.Empty;
                    _item.QuantityUnit = response.Data.QuantityUnit ?? string.Empty;
                }
            }
        }

        void Back_Click()
        {
            NavManager.NavigateTo(Paths.ItemHome);
        }

        async Task Save_Click()
        {
            await _form.Validate();
            if (_isFormValid)
            {
                var itemDto = new ItemCommandModel()
                {
                    Name = _item.Name,
                    Code = _item.Code,
                    Number = _item.Number,
                    Sku = _item.Sku,
                    QuantityUnit = _item.QuantityUnit
                };
                if (EditMode == EditMode.Add)
                {
                    var response = await ApiClient.AddItem(itemDto);
                    Snackbar.CheckSuccessFail(response);

                    if (response.IsSuccessful)
                    {
                        NavManager.NavigateTo(Paths.ItemHome);
                    }
                }
                else if (EditMode == EditMode.Update)
                {
                    var response = await ApiClient.UpdateItem(_item.Id, itemDto);
                    Snackbar.CheckSuccessFail(response);

                    if (response.IsSuccessful)
                    {
                        NavManager.NavigateTo(Paths.ItemHome);
                    }
                }

            }
        }

    }
}
