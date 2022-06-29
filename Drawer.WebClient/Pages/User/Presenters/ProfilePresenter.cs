using Drawer.Contract;
using Drawer.Contract.UserInformation;
using Drawer.WebClient.Api;
using Drawer.WebClient.Pages.User.Views;
using Drawer.WebClient.Presenters;
using MudBlazor;

namespace Drawer.WebClient.Pages.User.Presenters
{
    public class ProfilePresenter : SnackbarPresenter 
    {
        public ProfilePresenter(ApiClient apiClient, ISnackbar snackbar) : base(apiClient, snackbar)
        {
        }

        public IProfileView View { get; set; } = null!;

        public async Task LoadUserAsync()
        {
            var requstMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.User.Get);
            var apiResponse = await LoadAsync(new ApiRequestMessage<GetUserResponse>(requstMessage));
            if (apiResponse.IsSuccessful)
            {
                View.Model.Email = apiResponse.Data.Email;
                View.Model.DisplayName = apiResponse.Data.DisplayName;
            }
        }

        public async Task SaveUserAsync()
        {
            var requstMessage = new HttpRequestMessage(HttpMethod.Put, ApiRoutes.User.Update);
            requstMessage.Content = JsonContent.Create(new UpdateUserRequest(View.Model.DisplayName!));
            await SaveAsync(new ApiRequestMessage<Unit>(requstMessage));
        }
    }
}
