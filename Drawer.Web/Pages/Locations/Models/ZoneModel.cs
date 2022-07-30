using FluentValidation;

namespace Drawer.Web.Pages.Locations.Models
{
    public class ZoneModel
    {
        public long Id { get; set; }
        public long WorkPlaceId { get; set; }
        public string WorkplaceName { get; set; }
        public string? Name { get; set; }
        public string? Note { get; set; }
    }

    public class ZoneModelValidator : AbstractValidator<ZoneModel>
    {
        public ZoneModelValidator()
        {
            RuleFor(x => x.WorkPlaceId)
                .GreaterThan(0);

            RuleFor(x => x.Name)
                 .NotEmpty()
                 .Length(1, 100);
        }
    }
}
