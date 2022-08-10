using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Shared;
using Drawer.Web.Authentication;
using Drawer.Web.Utils;

namespace Drawer.Web.Api.Inventory
{
    public class InventoryItemApiClient : ApiClient
    {
        public InventoryItemApiClient(HttpClient httpClient, ITokenManager tokenManager) : base(httpClient, tokenManager)
        {
        }

        public async Task<ApiResponse<List<InventoryItemQueryModel>>> GetInventoryDetails(long? itemId = null, long? locationId = null)
        {
            var uri = ApiRoutes.InventoryItems.Get
                .AddQuery("ItemId", itemId?.ToString())
                .AddQuery("LocationId", locationId?.ToString());

            var request = new ApiRequest<List<InventoryItemQueryModel>>(
                HttpMethod.Get,
                uri);

            return await SendAsync(request);
        }

    }
}
