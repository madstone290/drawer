using Drawer.Contract;
using Drawer.Contract.Locations;
using Drawer.Web.Authentication;

namespace Drawer.Web.Api.Locations
{
    public class SpotApiClient : ApiClient
    {
        public SpotApiClient(HttpClient httpClient, ITokenManager tokenManager) : base(httpClient, tokenManager)
        {

        }

        public async Task<ApiResponse<CreateSpotResponse>> AddSpot(long zoneId, string name, string note)
        {
            var request = new ApiRequest<CreateSpotResponse>(
                HttpMethod.Post,
                ApiRoutes.Spots.Create,
                new CreateSpotRequest(zoneId, name, note));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> UpdateSpot(long id, string name, string note)
        {
            var request = new ApiRequest(
                HttpMethod.Put,
                ApiRoutes.Spots.Update.Replace("{id}", $"{id}"),
                new UpdateSpotRequest(name, note));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> DeleteSpot(long id)
        {
            var request = new ApiRequest(
                HttpMethod.Delete,
                ApiRoutes.Spots.Delete.Replace("{id}", $"{id}"));
                
            return await SendAsync(request);
        }

        public async Task<ApiResponse<GetSpotsResponse>> GetSpot(long id)
        {
            var request = new ApiRequest<GetSpotsResponse>(
                HttpMethod.Get,
                ApiRoutes.Spots.Get.Replace("{id}", $"{id}"));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<GetSpotsResponse>> GetSpots()
        {
            var request = new ApiRequest<GetSpotsResponse>(
                HttpMethod.Get, 
                ApiRoutes.Spots.GetList);

            return await SendAsync(request);
        }


    }
}
