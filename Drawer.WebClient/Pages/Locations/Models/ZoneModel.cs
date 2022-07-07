using FluentValidation;

namespace Drawer.WebClient.Pages.Locations.Models
{
    public class ZoneModel
    {
        public long Id { get; set; }
        public long WorkPlaceId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
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
