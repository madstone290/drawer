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

        public async Task<ApiResponse<CreateWorkplaceResponse>> AddWorkplace(CreateWorkplaceRequest content)
        {
            var request = new ApiRequest<CreateWorkplaceResponse>(
                HttpMethod.Post,
                ApiRoutes.Workplaces.Create,
                content);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<BatchCreateWorkplaceResponse>> BatchAddWorkplace(BatchCreateWorkplaceRequest content)
        {
            var request = new ApiRequest<BatchCreateWorkplaceResponse>(
                HttpMethod.Post,
                ApiRoutes.Workplaces.BatchCreate,
                content);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> UpdateWorkplace(long id, UpdateWorkplaceRequest content)
        {
            var request = new ApiRequest(
                HttpMethod.Put,
                ApiRoutes.Workplaces.Update.Replace("{id}", $"{id}"),
                content);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> DeleteWorkplace(long id)
        {
            var request = new ApiRequest(
                HttpMethod.Delete,
                ApiRoutes.Workplaces.Delete.Replace("{id}", $"{id}"));
                
            return await SendAsync(request);
        }

        public async Task<ApiResponse<GetWorkplaceResponse>> GetWorkplace(long id)
        {
            var request = new ApiRequest<GetWorkplaceResponse>(
                HttpMethod.Get,
                ApiRoutes.Workplaces.Get.Replace("{id}", $"{id}"));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<GetWorkplacesResponse>> GetWorkplaces()
        {
            var request = new ApiRequest<GetWorkplacesResponse>(
                HttpMethod.Get, 
                ApiRoutes.Workplaces.GetList);

            return await SendAsync(request);
        }


    }
}
