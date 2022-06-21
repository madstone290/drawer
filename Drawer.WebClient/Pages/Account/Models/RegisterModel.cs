using FluentValidation;

namespace Drawer.WebClient.Pages.Account.Models
{
    public class RegisterModel
    {
        public string? DisplayName { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? ConfirmPassword { get; set; }
    }

    public class RegisterModelValidator : AbstractValidator<RegisterModel>
    {
        public RegisterModelValidator()
        {
            RuleFor(x => x.DisplayName)
                .NotEmpty()
                .Length(1, 100);

            RuleFor(x => x.Email)
               .Cascade(CascadeMode.Stop)
               .NotEmpty()
               .EmailAddress();

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Length(8, 100);

            RuleFor(x => x.ConfirmPassword)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Length(8, 100)
                .Equal(x => x.Password)
                .WithMessage(Messages.PasswordDoesNotMatch);
        }

    }
}
