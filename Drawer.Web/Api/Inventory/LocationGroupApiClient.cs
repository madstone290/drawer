using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Shared;
using Drawer.Web.Authentication;

namespace Drawer.Web.Api.Inventory
{
    public class LocationGroupApiClient : ApiClient
    {
        public LocationGroupApiClient(HttpClient httpClient, ITokenManager tokenManager) : base(httpClient, tokenManager)
        {

        }

        public async Task<ApiResponse<LocationGroupQueryModel?>> GetLocationGroup(long id)
        {
            var request = new ApiRequest<LocationGroupQueryModel?>(
                HttpMethod.Get,
                ApiRoutes.LocationGroups.Get.Replace("{id}", $"{id}"));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<List<LocationGroupQueryModel>>> GetLocationGroups()
        {
            var request = new ApiRequest<List<LocationGroupQueryModel>>(
                HttpMethod.Get,
                ApiRoutes.LocationGroups.GetList);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<long>> AddLocationGroup(LocationGroupAddCommandModel location)
        {
            var request = new ApiRequest<long>(
                HttpMethod.Post,
                ApiRoutes.LocationGroups.Add,
                location);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<List<long>>> BatchAddLocationGroup(List<LocationGroupAddCommandModel> locationList)
        {
            var request = new ApiRequest<List<long>>(
                HttpMethod.Post,
                ApiRoutes.LocationGroups.BatchAdd,
                locationList);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> UpdateLocationGroup(long id, LocationGroupUpdateCommandModel location)
        {
            var request = new ApiRequest(
                HttpMethod.Put,
                ApiRoutes.LocationGroups.Update.Replace("{id}", $"{id}"),
                location);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> DeleteLocationGroup(long id)
        {
            var request = new ApiRequest(
                HttpMethod.Delete,
                ApiRoutes.LocationGroups.Remove.Replace("{id}", $"{id}"));

            return await SendAsync(request);
        }



    }
}
