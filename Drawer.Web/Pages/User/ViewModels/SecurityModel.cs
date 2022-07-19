using FluentValidation;

namespace Drawer.Web.Pages.User.ViewModels
{
    public class SecurityModel
    {
        public string Password { get; set; } = string.Empty;

        public string NewPassword { get; set; } = string.Empty;

        public string ConfirmNewPassword { get; set; } = string.Empty;
    }

    public class SecurityModelValidator : AbstractValidator<SecurityModel>
    {
        public SecurityModelValidator()
        {
            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Length(8, 100);

            RuleFor(x => x.NewPassword)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Length(8, 100);


            RuleFor(x => x.ConfirmNewPassword)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Length(8, 100)
                .Equal(x => x.NewPassword)
                .WithMessage(Messages.PasswordDoesNotMatch);
        }

    }
}
