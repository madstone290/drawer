using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Shared;
using Drawer.Web.Authentication;
using Drawer.Web.Utils;

namespace Drawer.Web.Api.Inventory
{
    public class IssueApiClient : ApiClient
    {
        public IssueApiClient(HttpClient httpClient, ITokenManager tokenManager) : base(httpClient, tokenManager)
        {

        }

        public async Task<ApiResponse<List<IssueQueryModel>>> GetIssues(DateTime from, DateTime to)
        {
            var request = new ApiRequest<List<IssueQueryModel>>(
                HttpMethod.Get,
                ApiRoutes.Issues.GetList
                    .AddQuery("From", from.ToString("yyyy-MM-dd"))
                    .AddQuery("To", to.ToString("yyy-MM-dd")));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<IssueQueryModel?>> GetIssue(long id)
        {
            var request = new ApiRequest<IssueQueryModel?>(
                HttpMethod.Get,
                ApiRoutes.Issues.Get.Replace("{id}", $"{id}"));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<long>> AddIssue(IssueCommandModel issue)
        {
            var request = new ApiRequest<long>(
                HttpMethod.Post,
                ApiRoutes.Issues.Add,
                issue);

            return await SendAsync(request);
        }


        public async Task<ApiResponse<List<long>>> BatchAddIssue(List<IssueCommandModel> issueList)
        {
            var request = new ApiRequest<List<long>>(
                HttpMethod.Post,
                ApiRoutes.Issues.BatchAdd,
                issueList);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> UpdateIssue(long id, IssueCommandModel issue)
        {
            var request = new ApiRequest(
                HttpMethod.Put,
                ApiRoutes.Issues.Update.Replace("{id}", $"{id}"),
                issue);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> RemoveIssue(long id)
        {
            var request = new ApiRequest(
                HttpMethod.Delete,
                ApiRoutes.Issues.Remove.Replace("{id}", $"{id}"));
                
            return await SendAsync(request);
        }

       

    }
}
