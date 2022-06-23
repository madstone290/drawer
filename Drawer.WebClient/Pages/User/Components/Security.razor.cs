using Drawer.WebClient.Pages.User.Presenters;
using Drawer.WebClient.Pages.User.ViewModels;
using Drawer.WebClient.Pages.User.Views;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace Drawer.WebClient.Pages.User.Components
{
    public partial class Security : ISecurityView
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

        [Inject] SecurityPresenter Presenter { get; set; } = null!;

        protected override Task OnInitializedAsync()
        {
            Presenter.View = this;
            return base.OnInitializedAsync();
        }

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
                await Presenter.ChanagePasswordAsync();
            }
        }

    }
}
