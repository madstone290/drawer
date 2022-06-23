using Drawer.Contract;
using Drawer.Contract.UserInformation;
using Drawer.WebClient.Api;
using Drawer.WebClient.Pages.User.Views;
using Drawer.WebClient.Presenters;
using MudBlazor;

namespace Drawer.WebClient.Pages.User.Presenters
{
    public class SecurityPresenter : SnackbarPresenter
    {
        public SecurityPresenter(ApiClient apiClient, ISnackbar snackbar) : base(apiClient, snackbar)
        {
        }

        public ISecurityView View { get; set; } = null!;

        public async Task ChanagePasswordAsync()
        {
            var requstMessage = new HttpRequestMessage(HttpMethod.Put, ApiRoutes.User.UpdatePassword);
            requstMessage.Content = JsonContent.Create(new UpdatePasswordRequest(View.Model.Password!, View.Model.NewPassword!));
            var apiRequest = new ApiRequestMessage(requstMessage);
            var apiResponse = await SaveAsync(apiRequest);
            if (apiResponse.IsSuccessful)
            {
                View.Model.Password = string.Empty;
                View.Model.NewPassword = string.Empty;
                View.Model.ConfirmNewPassword = string.Empty;
            }
        }
    }
}
