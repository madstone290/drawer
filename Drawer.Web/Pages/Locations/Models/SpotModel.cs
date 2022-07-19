using FluentValidation;

namespace Drawer.Web.Pages.Locations.Models
{
    public class SpotModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public long ZoneId { get; set; }
    }

    public class SpotModelValidator : AbstractValidator<SpotModel>
    {
        public SpotModelValidator()
        {
            RuleFor(x => x.ZoneId)
                .GreaterThan(0);

            RuleFor(x => x.Name)
                 .NotEmpty()
                 .Length(1, 100);
        }
    }
}
