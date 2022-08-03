using Drawer.Contract;
using Drawer.Contract.InventoryManagement;
using Drawer.Web.Authentication;

namespace Drawer.Web.Api.InventoryManagement
{
    public class LocationApiClient : ApiClient
    {
        public LocationApiClient(HttpClient httpClient, ITokenManager tokenManager) : base(httpClient, tokenManager)
        {

        }

        public async Task<ApiResponse<CreateLocationResponse>> AddLocation(CreateLocationRequest content)
        {
            var request = new ApiRequest<CreateLocationResponse>(
                HttpMethod.Post,
                ApiRoutes.Locations.Create,
                content);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<BatchCreateLocationResponse>> BatchAddLocation(BatchCreateLocationRequest content)
        {
            var request = new ApiRequest<BatchCreateLocationResponse>(
                HttpMethod.Post,
                ApiRoutes.Locations.BatchCreate,
                content);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> UpdateLocation(long id, UpdateLocationRequest content)
        {
            var request = new ApiRequest(
                HttpMethod.Put,
                ApiRoutes.Locations.Update.Replace("{id}", $"{id}"),
                content);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> DeleteLocation(long id)
        {
            var request = new ApiRequest(
                HttpMethod.Delete,
                ApiRoutes.Locations.Delete.Replace("{id}", $"{id}"));
                
            return await SendAsync(request);
        }

        public async Task<ApiResponse<GetLocationResponse>> GetLocation(long id)
        {
            var request = new ApiRequest<GetLocationResponse>(
                HttpMethod.Get,
                ApiRoutes.Locations.Get.Replace("{id}", $"{id}"));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<GetLocationsResponse>> GetLocations()
        {
            var request = new ApiRequest<GetLocationsResponse>(
                HttpMethod.Get, 
                ApiRoutes.Locations.GetList);

            return await SendAsync(request);
        }


    }
}
