using Drawer.Contract;
using Drawer.Contract.Locations;
using Drawer.Web.Authentication;

namespace Drawer.Web.Api.Locations
{
    public class ZoneApiClient : ApiClient
    {
        public ZoneApiClient(HttpClient httpClient, ITokenManager tokenManager) : base(httpClient, tokenManager)
        {

        }

        public async Task<ApiResponse<CreateZoneResponse>> AddZone(long workPlaceId, string name, string note)
        {
            var request = new ApiRequest<CreateZoneResponse>(
                HttpMethod.Post,
                ApiRoutes.Zones.Create,
                new CreateZoneRequest(workPlaceId, name, note));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> UpdateZone(long id, string name, string note)
        {
            var request = new ApiRequest(
                HttpMethod.Put,
                ApiRoutes.Zones.Update.Replace("{id}", $"{id}"),
                new UpdateZoneRequest(name, note));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> DeleteZone(long id)
        {
            var request = new ApiRequest(
                HttpMethod.Delete,
                ApiRoutes.Zones.Delete.Replace("{id}", $"{id}"));
                
            return await SendAsync(request);
        }

        public async Task<ApiResponse<GetZonesResponse>> GetZone(long id)
        {
            var request = new ApiRequest<GetZonesResponse>(
                HttpMethod.Get,
                ApiRoutes.Zones.Get.Replace("{id}", $"{id}"));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<GetZonesResponse>> GetZones()
        {
            var request = new ApiRequest<GetZonesResponse>(
                HttpMethod.Get, 
                ApiRoutes.Zones.GetList);

            return await SendAsync(request);
        }


    }
}
