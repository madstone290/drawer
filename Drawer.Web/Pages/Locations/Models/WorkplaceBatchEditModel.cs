using Drawer.Web.DataBinding;
using FluentValidation;

namespace Drawer.Web.Pages.Locations.Models
{
    public class WorkplaceBatchEditModel : BindingObject
    {
        public string? Name { get; set; }
        public string? Note { get; set; }

    }

    public class WorkplaceBatchEditModelValidator : AbstractValidator<WorkplaceBatchEditModel>
    {
        public WorkplaceBatchEditModelValidator()
        {
            RuleFor(x => x.Name)
                 .NotEmpty()
                 .WithMessage("이름은 필수입니다");
                 
        }
    }
}
