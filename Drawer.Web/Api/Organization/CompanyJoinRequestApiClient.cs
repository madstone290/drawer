using Drawer.Application.Services.Organization.CommandModels;
using Drawer.Application.Services.Organization.QueryModels;
using Drawer.Shared;
using Drawer.Web.Authentication;

namespace Drawer.Web.Api.Organization
{
    public class CompanyJoinRequestApiClient : ApiClient
    {
        public CompanyJoinRequestApiClient(HttpClient httpClient, ITokenManager tokenManager) : base(httpClient, tokenManager)
        {
        }

        public async Task<ApiResponse<List<CompanyJoinRequestQueryModel>>> GetRequests()
        {
            var request = new ApiRequest<List<CompanyJoinRequestQueryModel>>(
                HttpMethod.Get,
                ApiRoutes.JoinRequests.GetList);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> AddRequest(JoinRequestAddCommandModel joinRequest)
        {
            var request = new ApiRequest(
                   HttpMethod.Post,
                   ApiRoutes.JoinRequests.Add,
                   joinRequest);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> HandleRequest(long id, JoinRequestHandleCommandModel joinRequest)
        {
            var request = new ApiRequest(
                   HttpMethod.Put,
                   ApiRoutes.JoinRequests.Handle.Replace("{id}", $"{id}"),
                   joinRequest);

            return await SendAsync(request);
        }
    }
}
