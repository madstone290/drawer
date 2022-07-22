using Drawer.Web.Api.Items;
using Drawer.Web.Pages.Items.Views;
using Drawer.Web.Presenters;
using MudBlazor;

namespace Drawer.Web.Pages.Items.Presenters
{
    public class ItemEditPresenter : SnackbarPresenter
    {
        private readonly ItemApiClient _apiClient;

        public ItemEditPresenter(ISnackbar snackbar, ItemApiClient apiClient) : base(snackbar)
        {
            _apiClient = apiClient;
        }

        public IItemEditView View { get; set; } = null!;

        /// <summary>
        /// 작업장을 추가한다.
        /// </summary>
        /// <returns></returns>
        public async Task AddItemAsync()
        {
            var item = View.Item;
            var response = await _apiClient.AddItem(item.Name, item.Code, item.Number, item.Sku, item.QuantityUnit);
            CheckSuccessFail(response);

            if (response.IsSuccessful)
            {
                item.Id = response.Data.Id;
                View.CloseView();
            }
        }

        /// <summary>
        /// 작업장을 수정한다.
        /// </summary>
        /// <returns></returns>
        public async Task UpdateItemAsync()
        {
            var item = View.Item;
            var response = await _apiClient.UpdateItem(item.Id, item.Name, item.Code, item.Number, item.Sku, item.QuantityUnit);
            CheckSuccessFail(response);

            if (response.IsSuccessful)
            {
                View.CloseView();
            }
        }
    }
}
