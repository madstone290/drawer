using Drawer.Contract;
using Drawer.Contract.Common;
using Drawer.Contract.UserInformation;
using Drawer.WebClient.Pages.Account.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace Drawer.WebClient.Pages.Account.Components
{
    public partial class Security
    {
        public SecurityModel Model { get; set; } = new SecurityModel();
        public SecurityModelValidator Validator { get; set; } = new SecurityModelValidator();
        public MudForm Form { get; set; } = null!;
        public bool FormIsValid { get; set; }

        private bool _passwordVisible = false;
        public InputType PasswordInputType => _passwordVisible ? InputType.Text : InputType.Password;
        public string PasswordIcon => _passwordVisible ? Icons.Material.Filled.Visibility : Icons.Material.Filled.VisibilityOff;

        private bool _newPasswordVisible = false;
        public InputType NewPasswordInputType => _newPasswordVisible ? InputType.Text : InputType.Password;
        public string NewPasswordIcon => _newPasswordVisible ? Icons.Material.Filled.Visibility : Icons.Material.Filled.VisibilityOff;

        private bool _confirmNewPasswordVisible = false;
        public InputType ConfirmNewPasswordInputType => _confirmNewPasswordVisible ? InputType.Text : InputType.Password;
        public string ConfirmNewPasswordIcon => _confirmNewPasswordVisible ? Icons.Material.Filled.Visibility : Icons.Material.Filled.VisibilityOff;


        [Inject]
        public HttpClient HttpClient { get; set; } = null!;
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

        public string? ErrorText { get; set; }

        void TogglePasswordVisibility()
        {
            _passwordVisible = !_passwordVisible;
        }

        void ToggleNewPasswordVisibility()
        {
            _newPasswordVisible = !_newPasswordVisible;
        }

        void ToggleConfirmNewPasswordVisibility()
        {
            _confirmNewPasswordVisible = !_confirmNewPasswordVisible;
        }

        async Task ChanagePasswordClickAsync()
        {
            await Form.Validate();
            if (Form.IsValid)
            {
                var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                var at = state.User.Claims.FirstOrDefault(x => x.Type == "AccessToken").Value;

                var requstMessage = new HttpRequestMessage(HttpMethod.Put, ApiRoutes.User.UpdatePassword);
                requstMessage.Headers.Add("Authorization", $"bearer {at}");
                requstMessage.Content = JsonContent.Create(new UpdatePasswordRequest(Model.Password!, Model.NewPassword!));
                var responseMessage = await HttpClient.SendAsync(requstMessage);

                if (responseMessage.IsSuccessStatusCode)
                {
                    //snackbar
                }
                else if (responseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest
                  || responseMessage.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    var error = await responseMessage.Content.ReadFromJsonAsync<ErrorResponse>();
                    ErrorText = error?.Message;
                }
                else
                {
                    ErrorText = responseMessage.StatusCode.ToString();
                }

            }
        }

    }
}
