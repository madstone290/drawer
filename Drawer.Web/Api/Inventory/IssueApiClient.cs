using Drawer.Contract;
using Drawer.Contract.Inventory;
using Drawer.Web.Authentication;
using Drawer.Web.Utils;

namespace Drawer.Web.Api.Inventory
{
    public class IssueApiClient : ApiClient
    {
        public IssueApiClient(HttpClient httpClient, ITokenManager tokenManager) : base(httpClient, tokenManager)
        {

        }

        public async Task<ApiResponse<GetIssuesResponse>> GetIssues(DateTime from, DateTime to)
        {
            var request = new ApiRequest<GetIssuesResponse>(
                HttpMethod.Get,
                ApiRoutes.Issues.GetList
                    .AddQuery("From", from.ToString("yyyy-MM-dd"))
                    .AddQuery("To", to.ToString("yyy-MM-dd")));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<GetIssueResponse>> GetIssue(long id)
        {
            var request = new ApiRequest<GetIssueResponse>(
                HttpMethod.Get,
                ApiRoutes.Issues.Get.Replace("{id}", $"{id}"));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<CreateIssueResponse>> AddIssue(CreateIssueRequest content)
        {
            var request = new ApiRequest<CreateIssueResponse>(
                HttpMethod.Post,
                ApiRoutes.Issues.Create,
                content);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> UpdateIssue(long id, UpdateIssueRequest content)
        {
            var request = new ApiRequest(
                HttpMethod.Put,
                ApiRoutes.Issues.Update.Replace("{id}", $"{id}"),
                content);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> DeleteIssue(long id)
        {
            var request = new ApiRequest(
                HttpMethod.Delete,
                ApiRoutes.Issues.Delete.Replace("{id}", $"{id}"));
                
            return await SendAsync(request);
        }

       

    }
}
