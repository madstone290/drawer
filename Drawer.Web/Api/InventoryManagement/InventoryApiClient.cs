using Drawer.Contract;
using Drawer.Contract.InventoryManagement;
using Drawer.Web.Authentication;
using Drawer.Web.Utils;

namespace Drawer.Web.Api.InventoryManagement
{
    public class InventoryApiClient : ApiClient
    {
        public InventoryApiClient(HttpClient httpClient, ITokenManager tokenManager) : base(httpClient, tokenManager)
        {
        }

        public async Task<ApiResponse<GetInventoryResponse>> GetInventoryDetails(long? itemId = null, long? locationId = null)
        {
            var uri = ApiRoutes.Inventory.Get
                .AddQuery("ItemId", itemId?.ToString())
                .AddQuery("LocationId", locationId?.ToString());

            var request = new ApiRequest<GetInventoryResponse>(
                HttpMethod.Get,
                uri);

            return await SendAsync(request);
        }

    }
}
