using Drawer.Contract;
using Drawer.Contract.Locations;
using Drawer.WebClient.Authentication;

namespace Drawer.WebClient.Api.Locations
{
    public class WorkPlaceApiClient : ApiClient
    {
        public WorkPlaceApiClient(HttpClient httpClient, ITokenManager tokenManager) : base(httpClient, tokenManager)
        {

        }

        public async Task<ApiResponse<CreateWorkPlaceResponse>> AddWorkPlace(string name, string note)
        {
            var request = new ApiRequest<CreateWorkPlaceResponse>(
                HttpMethod.Post,
                ApiRoutes.WorkPlaces.Create,
                new CreateWorkPlaceRequest(name, note));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> UpdateWorkPlace(long id, string name, string note)
        {
            var request = new ApiRequest(
                HttpMethod.Put,
                ApiRoutes.WorkPlaces.Update.Replace("{id}", $"{id}"),
                new UpdateWorkPlaceRequest(name, note));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> DeleteWorkPlace(long id)
        {
            var request = new ApiRequest(
                HttpMethod.Delete,
                ApiRoutes.WorkPlaces.Delete.Replace("{id}", $"{id}"));
                
            return await SendAsync(request);
        }

        public async Task<ApiResponse<GetWorkPlacesResponse>> GetWorkPlace(long id)
        {
            var request = new ApiRequest<GetWorkPlacesResponse>(
                HttpMethod.Get,
                ApiRoutes.WorkPlaces.Get.Replace("{id}", $"{id}"));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<GetWorkPlacesResponse>> GetWorkPlaces()
        {
            var request = new ApiRequest<GetWorkPlacesResponse>(
                HttpMethod.Get, 
                ApiRoutes.WorkPlaces.GetList);

            return await SendAsync(request);
        }


    }
}
