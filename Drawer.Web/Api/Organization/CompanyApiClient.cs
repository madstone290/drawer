using Drawer.Application.Services.Organization.CommandModels;
using Drawer.Application.Services.Organization.QueryModels;
using Drawer.Shared;
using Drawer.Web.Authentication;

namespace Drawer.Web.Api.Organization
{
    public class CompanyApiClient : ApiClient
    {
        public CompanyApiClient(HttpClient httpClient, ITokenManager tokenManager) : base(httpClient, tokenManager)
        {
        }

        public async Task<ApiResponse<CompanyQueryModel>> GetCompany()
        {
            var request = new ApiRequest<CompanyQueryModel>(
                HttpMethod.Get,
                ApiRoutes.Company.Get);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<string>> CreateCompany(CompanyAddUpdateCommandModel company)
        {
            var request = new ApiRequest<string>(
                   HttpMethod.Post,
                   ApiRoutes.Company.Create,
                   company);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> UpdateCompany(CompanyAddUpdateCommandModel company)
        {
            var request = new ApiRequest(
                   HttpMethod.Put,
                   ApiRoutes.Company.Update,
                   company);

            return await SendAsync(request);
        }


    }
}
