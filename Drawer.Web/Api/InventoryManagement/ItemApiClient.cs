using Drawer.Contract;
using Drawer.Contract.Inventory;
using Drawer.Web.Authentication;

namespace Drawer.Web.Api.InventoryManagement
{
    public class ItemApiClient : ApiClient
    {
        public ItemApiClient(HttpClient httpClient, ITokenManager tokenManager) : base(httpClient, tokenManager)
        {

        }

        public async Task<ApiResponse<BatchCreateItemResponse>> BatchAddItem(BatchCreateItemRequest content)
        {
            var request = new ApiRequest<BatchCreateItemResponse>(
                HttpMethod.Post,
                ApiRoutes.Items.BatchCreate,
                content);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<CreateItemResponse>> AddItem(string name, string? code, string? number,
            string? sku, string? measurementUnit)
        {
            var request = new ApiRequest<CreateItemResponse>(
                HttpMethod.Post,
                ApiRoutes.Items.Create,
                new CreateItemRequest(name, code, number, sku, measurementUnit));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> UpdateItem(long id, string name, string? code, string? number,
            string? sku, string? measurementUnit)
        {
            var request = new ApiRequest(
                HttpMethod.Put,
                ApiRoutes.Items.Update.Replace("{id}", $"{id}"),
                new UpdateItemRequest(name, code, number, sku, measurementUnit));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> DeleteItem(long id)
        {
            var request = new ApiRequest(
                HttpMethod.Delete,
                ApiRoutes.Items.Delete.Replace("{id}", $"{id}"));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<GetItemResponse>> GetItem(long id)
        {
            var request = new ApiRequest<GetItemResponse>(
                HttpMethod.Get,
                ApiRoutes.Items.Get.Replace("{id}", $"{id}"));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<GetItemsResponse>> GetItems()
        {
            var request = new ApiRequest<GetItemsResponse>(
                HttpMethod.Get,
                ApiRoutes.Items.GetList);

            return await SendAsync(request);
        }


    }
}
