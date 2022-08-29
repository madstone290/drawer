using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Shared;
using Drawer.Web.Authentication;
using Drawer.Web.Utils;

namespace Drawer.Web.Api.Inventory
{
    public class LayoutApiClient : ApiClient
    {
        public LayoutApiClient(HttpClient httpClient, ITokenManager tokenManager) : base(httpClient, tokenManager)
        {

        }

        public async Task<ApiResponse<LayoutQueryModel?>> GetLayout(long id)
        {
            var request = new ApiRequest<LayoutQueryModel?>(
                HttpMethod.Get,
                ApiRoutes.Layouts.Get.Replace("{id}", $"{id}"));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<LayoutQueryModel?>> GetLayoutByLocation(long locationId)
        {
            var request = new ApiRequest<LayoutQueryModel?>(
                HttpMethod.Get,
                ApiRoutes.Layouts.GetByLocation.Replace("{locationId}", $"{locationId}"));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<List<LayoutQueryModel>>> GetLayouts()
        {
            var request = new ApiRequest<List<LayoutQueryModel>>(
                HttpMethod.Get,
                ApiRoutes.Layouts.GetList);
                    
            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> EditLayout(LayoutEditCommandModel layout)
        {
            var request = new ApiRequest(
                HttpMethod.Post,
                ApiRoutes.Layouts.Edit,
                layout);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> DeleteLayout(long id)
        {
            var request = new ApiRequest(
                HttpMethod.Delete,
                ApiRoutes.Layouts.Remove.Replace("{id}", $"{id}"));

            return await SendAsync(request);
        }



    }
}
