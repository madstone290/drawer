﻿using FluentValidation;


namespace Drawer.WebClient.Pages.Organization.Models
{
    public class EditCompanyModel
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public EditCompanyModel() { }
        public EditCompanyModel(string id, string name, string phoneNumber)
        {
            Id = id;
            Name = name;
            PhoneNumber = phoneNumber;
        }
    }

    public class EditCompanyModelValidator : AbstractValidator<EditCompanyModel>
    {
        public EditCompanyModelValidator()
        {
            RuleFor(x => x.Name)
                 .NotEmpty()
                 .Length(1, 100);
        }
    }
}