using FluentValidation;


namespace Drawer.Web.Pages.Organization.Models
{
    public class EditCompanyModel
    {
        public long Id { get; set; } 

        public string Name { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public EditCompanyModel() { }
        public EditCompanyModel(long id, string name, string phoneNumber)
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
