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

        public async Task<ApiResponse<CreateSpotResponse>> AddSpot(CreateSpotRequest content)
        {
            var request = new ApiRequest<CreateSpotResponse>(
                HttpMethod.Post,
                ApiRoutes.Spots.Create,
                content);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<BatchCreateSpotResponse>> BatchAddSpot(BatchCreateSpotRequest content)
        {
            var request = new ApiRequest<BatchCreateSpotResponse>(
                HttpMethod.Post,
                ApiRoutes.Spots.BatchCreate,
                content);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> UpdateSpot(long id, UpdateSpotRequest content)
        {
            var request = new ApiRequest(
                HttpMethod.Put,
                ApiRoutes.Spots.Update.Replace("{id}", $"{id}"),
                content);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> DeleteSpot(long id)
        {
            var request = new ApiRequest(
                HttpMethod.Delete,
                ApiRoutes.Spots.Delete.Replace("{id}", $"{id}"));
                
            return await SendAsync(request);
        }

        public async Task<ApiResponse<GetSpotResponse>> GetSpot(long id)
        {
            var request = new ApiRequest<GetSpotResponse>(
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
