using Drawer.WebClient.Utils;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.WebClient.Pages.Account
{
    public partial class Register
    {
        public class InputModel
        {
            public string? DisplayName { get; set; }

            public string? Email { get; set; }

            public string? Password1 { get; set; }

            public string? Password2 { get; set; }
        }

        public class InputModelValidator : AbstractValidator<InputModel>
        {
            public InputModelValidator()
            {
                RuleFor(x => x.DisplayName)
                    .NotEmpty()
                    .Length(1, 100);

                RuleFor(x => x.Email)
                   .Cascade(CascadeMode.Stop)
                   .NotEmpty()
                   .EmailAddress();

                RuleFor(x => x.Password1)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty()
                    .Length(8, 100);

                RuleFor(x => x.Password2)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty()
                    .Length(8, 100)
                    .Equal(x=> x.Password1)
                    .WithMessage(Messages.PasswordDoesNotMatch);
            }

        }


        public InputModel Input { get; set; } = new InputModel();
        public InputModelValidator Validator { get; set; } = new InputModelValidator();
        public MudForm Form { get; set; } = null!;

        [Inject] NavigationManager NavigationManager { get; set; } = null!;

        async Task SubmitAsync()
        {
            await Form.Validate();

            if (Form.IsValid)
            {
                // 회원가입 진행
                var navigationUri = Paths.Account.RegisterHandler
                    .AddQueryParam("returnUri", Paths.Account.Register)
                    .AddQueryParam("redirectUri", Paths.Account.EmailSent)
                    .AddQueryParam("displayName", Input.DisplayName!)
                    .AddQueryParam("email", Input.Email!)
                    .AddQueryParam("password", Input.Password1!);

                NavigationManager.NavigateTo(navigationUri, true);
            }
        }


    }
}
