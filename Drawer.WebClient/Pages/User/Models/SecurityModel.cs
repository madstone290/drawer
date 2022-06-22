using FluentValidation;

namespace Drawer.WebClient.Pages.User.Models
{
    public class SecurityModel
    {
        public string? Password { get; set; }

        public string? NewPassword { get; set; }

        public string? ConfirmNewPassword { get; set; }
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
