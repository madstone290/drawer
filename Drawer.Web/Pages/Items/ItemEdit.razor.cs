using Drawer.Web.Api.Items;
using Drawer.Web.Pages.Items.Models;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.Web.Pages.Items
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

        [Parameter]
        public EditMode EditMode { get; set; }

        [Parameter]
        public long? ItemId { get; set; }

        [Inject]
        public ItemApiClient ApiClient { get; set; } = null!;

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

        void Cancel_Click()
        {
            NavManager.NavigateTo(Paths.Items.Home);
        }

        async Task Save_Click()
        {
            await _form.Validate();
            if (_isFormValid)
            {
                if (EditMode == EditMode.Add)
                {
                    var response = await ApiClient.AddItem(_item.Name, _item.Code, _item.Number, _item.Sku, _item.QuantityUnit);
                    Snackbar.CheckSuccessFail(response);

                    if (response.IsSuccessful)
                    {
                        _item.Id = response.Data.Id;
                        NavManager.NavigateTo(Paths.Items.Home);
                    }
                }
                else if (EditMode == EditMode.Update)
                {
                    var response = await ApiClient.UpdateItem(_item.Id, _item.Name, _item.Code, _item.Number, _item.Sku, _item.QuantityUnit);
                    Snackbar.CheckSuccessFail(response);

                    if (response.IsSuccessful)
                    {
                        NavManager.NavigateTo(Paths.Items.Home);
                    }
                }

            }
        }

    }
}
