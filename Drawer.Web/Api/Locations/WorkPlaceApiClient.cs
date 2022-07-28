using Drawer.Contract;
using Drawer.Contract.Locations;
using Drawer.Web.Authentication;

namespace Drawer.Web.Api.Locations
{
    public class WorkplaceApiClient : ApiClient
    {
        public WorkplaceApiClient(HttpClient httpClient, ITokenManager tokenManager) : base(httpClient, tokenManager)
        {

        }

        public async Task<ApiResponse<CreateWorkPlaceResponse>> AddWorkplace(CreateWorkPlaceRequest content)
        {
            var request = new ApiRequest<CreateWorkPlaceResponse>(
                HttpMethod.Post,
                ApiRoutes.WorkPlaces.Create,
                content);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> UpdateWorkplace(long id, UpdateWorkPlaceRequest content)
        {
            var request = new ApiRequest(
                HttpMethod.Put,
                ApiRoutes.WorkPlaces.Update.Replace("{id}", $"{id}"),
                content);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> DeleteWorkplace(long id)
        {
            var request = new ApiRequest(
                HttpMethod.Delete,
                ApiRoutes.WorkPlaces.Delete.Replace("{id}", $"{id}"));
                
            return await SendAsync(request);
        }

        public async Task<ApiResponse<GetWorkPlaceResponse>> GetWorkplace(long id)
        {
            var request = new ApiRequest<GetWorkPlaceResponse>(
                HttpMethod.Get,
                ApiRoutes.WorkPlaces.Get.Replace("{id}", $"{id}"));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<GetWorkPlacesResponse>> GetWorkplaces()
        {
            var request = new ApiRequest<GetWorkPlacesResponse>(
                HttpMethod.Get, 
                ApiRoutes.WorkPlaces.GetList);

            return await SendAsync(request);
        }


    }
}
