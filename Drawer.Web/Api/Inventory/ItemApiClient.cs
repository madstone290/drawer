using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Shared;
using Drawer.Web.Authentication;

namespace Drawer.Web.Api.Inventory
{
    public class ItemApiClient : ApiClient
    {
        public ItemApiClient(HttpClient httpClient, ITokenManager tokenManager) : base(httpClient, tokenManager)
        {

        }

        public async Task<ApiResponse<List<long>>> BatchAddItem(List<ItemCommandModel> itemList)
        {
            var request = new ApiRequest<List<long>>(
                HttpMethod.Post,
                ApiRoutes.Items.BatchCreate,
                itemList);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<long>> AddItem(ItemCommandModel item)
        {
            var request = new ApiRequest<long>(
                HttpMethod.Post,
                ApiRoutes.Items.Create,
                item);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> UpdateItem(long id, ItemCommandModel item)
        {
            var request = new ApiRequest(
                HttpMethod.Put,
                ApiRoutes.Items.Update.Replace("{id}", $"{id}"),
                item);  

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> DeleteItem(long id)
        {
            var request = new ApiRequest(
                HttpMethod.Delete,
                ApiRoutes.Items.Delete.Replace("{id}", $"{id}"));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<ItemQueryModel?>> GetItem(long id)
        {
            var request = new ApiRequest<ItemQueryModel?>(
                HttpMethod.Get,
                ApiRoutes.Items.Get.Replace("{id}", $"{id}"));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<List<ItemQueryModel>>> GetItems()
        {
            var request = new ApiRequest<List<ItemQueryModel>>(
                HttpMethod.Get,
                ApiRoutes.Items.GetList);

            return await SendAsync(request);
        }


    }
}
