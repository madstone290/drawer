using Drawer.WebClient.Api.UserInformation;
using Drawer.WebClient.Pages.User.Views;
using Drawer.WebClient.Presenters;
using MudBlazor;

namespace Drawer.WebClient.Pages.User.Presenters
{
    public class SecurityPresenter : SnackbarPresenter
    {
        private readonly UserApiClient _apiClient;

        public SecurityPresenter(ISnackbar snackbar, UserApiClient apiClient) : base(snackbar)
        {
            _apiClient = apiClient;
        }

        public ISecurityView View { get; set; } = null!;

        public async Task ChanagePasswordAsync()
        {
            var response = await _apiClient.ChangePassword(View.Model.Password, View.Model.NewPassword);
            CheckSuccessFail(response);
            
            if (response.IsSuccessful)
            {
                View.Model.Password = string.Empty;
                View.Model.NewPassword = string.Empty;
                View.Model.ConfirmNewPassword = string.Empty;
            }
        }
    }
}
