﻿using FluentValidation;

namespace Drawer.Web.Pages.User.ViewModels
{
    public class ProfileModel
    {
        public string? DisplayName { get; set; }

        public string? Email { get; set; }
    }

    public class ProfileModelValidator : AbstractValidator<ProfileModel>
    {
        public ProfileModelValidator()
        {
            RuleFor(x => x.DisplayName)
                 .NotEmpty()
                 .Length(1, 100);

            RuleFor(x => x.Email)
                   .Cascade(CascadeMode.Stop)
                   .NotEmpty()
                   .EmailAddress();
        }
    }
}
