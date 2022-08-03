using FluentValidation;

namespace Drawer.Web.Pages.Location.Models
{
    public class LocationModel
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Note { get; set; }
        public long UpperLocationId { get; set; }
        public string? UpperLocationName { get; set; }
        public int HierarchyLevel { get; set; }
    }

    public class LocationModelValidator : AbstractValidator<LocationModel>
    {
        public LocationModelValidator()
        {
            RuleFor(x => x.Name)
                 .NotEmpty()
                 .WithMessage("* 필수");
        }
    }
}
