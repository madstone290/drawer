using Drawer.Application.Services.UserInformation.CommandModels;
using Drawer.Web.Api.UserInformation;
using Drawer.Web.Pages.User.Views;
using Drawer.Web.Presenters;
using MudBlazor;

namespace Drawer.Web.Pages.User.Presenters
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
            var passwordDto = new UserPasswordCommandModel()
            {
                Password = View.Model.Password,
                NewPassword = View.Model.NewPassword
            };
            var response = await _apiClient.ChangePassword(passwordDto);
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
