using Drawer.Application.Services.UserInformation.CommandModels;
using Drawer.Application.Services.UserInformation.QueryModels;
using Drawer.Shared;
using Drawer.Web.Authentication;

namespace Drawer.Web.Api.UserInformation
{
    public class UserApiClient : ApiClient
    {
        public UserApiClient(HttpClient httpClient, ITokenManager tokenManager) : base(httpClient, tokenManager)
        {
        }

        public async Task<ApiResponse<UserQueryModel>> GetUser()
        {
            var request = new ApiRequest<UserQueryModel>(
                  HttpMethod.Get,
                  ApiRoutes.User.Get);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> SaveUser(UserCommandModel userInfo)
        {
            var request = new ApiRequest<Unit>(
                    HttpMethod.Put,
                    ApiRoutes.User.Update,
                    userInfo);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> ChangePassword(UserPasswordCommandModel userPassword)
        {
            var request = new ApiRequest<Unit>(
                      HttpMethod.Put,
                      ApiRoutes.User.UpdatePassword,
                      userPassword);

            return await SendAsync(request);
        }
    }
}
