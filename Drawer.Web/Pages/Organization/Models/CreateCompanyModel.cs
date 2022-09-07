using FluentValidation;

namespace Drawer.Web.Pages.Organization.Models
{
    public class CreateCompanyModel
    {
        public string Name { get; set; } = string.Empty;

        public class Validator : AbstractValidator<CreateCompanyModel>
        {
            public Validator()
            {
                RuleFor(x => x.Name)
                     .NotEmpty()
                     .Length(1, 100);
            }
        }
    }
}
