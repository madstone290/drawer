using Drawer.Web.Api.Items;
using Drawer.Web.Pages.Items.Views;
using Drawer.Web.Presenters;
using MudBlazor;

namespace Drawer.Web.Pages.Items.Presenters
{
    public class ItemBatchEditPresenter : SnackbarPresenter
    {
        private readonly ItemApiClient _apiClient;

        public ItemBatchEditPresenter(ISnackbar snackbar, ItemApiClient apiClient) : base(snackbar)
        {
            _apiClient = apiClient;
        }

        public IItemBatchEditView View { get; set; } = null!;

        /// <summary>
        /// 작업장을 추가한다.
        /// </summary>
        /// <returns></returns>
        public async Task AddItemsAsync()
        {
            if (!View.IsDataValid) 
            { 
                _snackbar.Add("데이터가 유효하지 않습니다");
                return;
            }

            var itemList = View.ItemList;
            foreach(var item in itemList)
            {
                // validate
                var response = await _apiClient.AddItem(item.Name, item.Code, item.Number, item.Sku, item.QuantityUnit);
                CheckSuccessFail(response);
            }

            View.IsSavingEnabled = false;
        }
    }
}
