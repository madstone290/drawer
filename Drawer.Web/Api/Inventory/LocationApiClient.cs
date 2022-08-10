using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Shared;
using Drawer.Web.Authentication;

namespace Drawer.Web.Api.Inventory
{
    public class LocationApiClient : ApiClient
    {
        public LocationApiClient(HttpClient httpClient, ITokenManager tokenManager) : base(httpClient, tokenManager)
        {

        }

        public async Task<ApiResponse<LocationQueryModel?>> GetLocation(long id)
        {
            var request = new ApiRequest<LocationQueryModel?>(
                HttpMethod.Get,
                ApiRoutes.Locations.Get.Replace("{id}", $"{id}"));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<List<LocationQueryModel>>> GetLocations()
        {
            var request = new ApiRequest<List<LocationQueryModel>>(
                HttpMethod.Get,
                ApiRoutes.Locations.GetList);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<long>> AddLocation(LocationAddCommandModel location)
        {
            var request = new ApiRequest<long>(
                HttpMethod.Post,
                ApiRoutes.Locations.Create,
                location);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<List<long>>> BatchAddLocation(List<LocationAddCommandModel> locationList)
        {
            var request = new ApiRequest<List<long>>(
                HttpMethod.Post,
                ApiRoutes.Locations.BatchCreate,
                locationList);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> UpdateLocation(long id, LocationUpdateCommandModel location)
        {
            var request = new ApiRequest(
                HttpMethod.Put,
                ApiRoutes.Locations.Update.Replace("{id}", $"{id}"),
                location);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> DeleteLocation(long id)
        {
            var request = new ApiRequest(
                HttpMethod.Delete,
                ApiRoutes.Locations.Delete.Replace("{id}", $"{id}"));

            return await SendAsync(request);
        }



    }
}
