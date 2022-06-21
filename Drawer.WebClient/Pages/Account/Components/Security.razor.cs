using Drawer.WebClient.Pages.Account.Models;
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
        public string PasswordIcon => _passwordVisible? Icons.Material.Filled.Visibility : Icons.Material.Filled.VisibilityOff;

        private bool _newPasswordVisible = false;
        public InputType NewPasswordInputType => _newPasswordVisible ? InputType.Text : InputType.Password;
        public string NewPasswordIcon => _newPasswordVisible ? Icons.Material.Filled.Visibility : Icons.Material.Filled.VisibilityOff;

        private bool _confirmNewPasswordVisible = false;
        public InputType ConfirmNewPasswordInputType => _confirmNewPasswordVisible ? InputType.Text : InputType.Password;
        public string ConfirmNewPasswordIcon => _confirmNewPasswordVisible ? Icons.Material.Filled.Visibility : Icons.Material.Filled.VisibilityOff;


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

        void ChanagePasswordClick()
        {

        }

    }
}
