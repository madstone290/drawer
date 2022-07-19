using FluentValidation;

namespace Drawer.Web.Pages.Locations.Models
{
    public class WorkPlaceModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
    }

    public class WorkPlaceModelValidator : AbstractValidator<WorkPlaceModel>
    {
        public WorkPlaceModelValidator()
        {
            RuleFor(x => x.Name)
                 .NotEmpty()
                 .Length(1, 100);
        }
    }
}
