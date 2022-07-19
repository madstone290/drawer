using Drawer.Contract;
using Drawer.Contract.Organization;
using Drawer.Web.Authentication;

namespace Drawer.Web.Api.Organization
{
    public class CompanyApiClient : ApiClient
    {
        public CompanyApiClient(HttpClient httpClient, ITokenManager tokenManager) : base(httpClient, tokenManager)
        {
        }

        public async Task<ApiResponse<GetCompanyResponse>> GetCompany()
        {
            var request = new ApiRequest<GetCompanyResponse>(
                HttpMethod.Get,
                ApiRoutes.Company.Get);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<CreateCompanyResponse>> CreateCompany(string name, string phoneNumber)
        {
            var request = new ApiRequest<CreateCompanyResponse>(
                   HttpMethod.Post,
                   ApiRoutes.Company.Create,
                   new CreateCompanyRequest(name, phoneNumber));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> UpdateCompany(string name, string phoneNumber)
        {
            var request = new ApiRequest(
                   HttpMethod.Put,
                   ApiRoutes.Company.Update,
                   new UpdateCompanyRequest(name, phoneNumber));

            return await SendAsync(request);
        }


    }
}
