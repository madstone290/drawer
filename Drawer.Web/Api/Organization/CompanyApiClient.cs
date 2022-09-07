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

        public async Task<ApiResponse<long>> CreateCompany(CompanyCommandModel company)
        {
            var request = new ApiRequest<long>(
                   HttpMethod.Post,
                   ApiRoutes.Company.Add,
                   company);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> UpdateCompany(CompanyCommandModel company)
        {
            var request = new ApiRequest(
                   HttpMethod.Put,
                   ApiRoutes.Company.Update,
                   company);

            return await SendAsync(request);
        }


        public async Task<ApiResponse<Unit>> AddMember(MemberCommandModel member)
        {
            var request = new ApiRequest(
                   HttpMethod.Post,
                   ApiRoutes.Company.AddMember,
                   member);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> RemoveMember(MemberCommandModel member)
        {
            var request = new ApiRequest(
                   HttpMethod.Delete,
                   ApiRoutes.Company.RemoveMemeber,
                   member);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<List<CompanyMemberQueryModel>>> GetMembers()
        {
            var request = new ApiRequest<List<CompanyMemberQueryModel>>(
                   HttpMethod.Get,
                   ApiRoutes.Company.GetMembers);

            return await SendAsync(request);
        }
    }
}
