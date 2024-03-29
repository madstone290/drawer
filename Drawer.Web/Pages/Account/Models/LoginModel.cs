﻿using FluentValidation;

namespace Drawer.Web.Pages.Account.Models
{
    public class LoginModel
    {
        public string? Email { get; set; }

        public string? Password { get; set; }

        public bool RememberEmail { get; set; }
    }

    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Length(8, 100);
        }
    }
}
