using Drawer.Contract;
using Drawer.Contract.UserInformation;
using Drawer.WebClient.Authentication;

namespace Drawer.WebClient.Api.UserInformation
{
    public class UserApiClient : ApiClient
    {
        public UserApiClient(HttpClient httpClient, ITokenManager tokenManager) : base(httpClient, tokenManager)
        {
        }

        public async Task<ApiResponse<GetUserResponse>> GetUser()
        {
            var request = new ApiRequest<GetUserResponse>(
                  HttpMethod.Get,
                  ApiRoutes.User.Get);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> SaveUser(string displayName)
        {
            var request = new ApiRequest<Unit>(
                    HttpMethod.Put,
                    ApiRoutes.User.Update,
                    new UpdateUserRequest(displayName));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> ChangePassword(string password, string newPassword)
        {
            var request = new ApiRequest<Unit>(
                      HttpMethod.Put,
                      ApiRoutes.User.UpdatePassword,
                      new UpdatePasswordRequest(password, newPassword));

            return await SendAsync(request);
        }
    }
}
