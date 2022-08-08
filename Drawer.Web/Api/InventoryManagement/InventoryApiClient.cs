using Drawer.Contract;
using Drawer.Contract.Inventory;
using Drawer.Web.Authentication;
using Drawer.Web.Utils;

namespace Drawer.Web.Api.InventoryManagement
{
    public class InventoryApiClient : ApiClient
    {
        public InventoryApiClient(HttpClient httpClient, ITokenManager tokenManager) : base(httpClient, tokenManager)
        {
        }

        public async Task<ApiResponse<GetInventoryItemsResponse>> GetInventoryDetails(long? itemId = null, long? locationId = null)
        {
            var uri = ApiRoutes.InventoryItems.Get
                .AddQuery("ItemId", itemId?.ToString())
                .AddQuery("LocationId", locationId?.ToString());

            var request = new ApiRequest<GetInventoryItemsResponse>(
                HttpMethod.Get,
                uri);

            return await SendAsync(request);
        }

    }
}
