using FluentValidation;

namespace Drawer.Web.Pages.Locations.Models
{
    public class SpotBatchEditModel
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Note { get; set; }
        public long ZoneId { get; set; }
    }

    public class SpotBatchEditModelValidator : AbstractValidator<SpotBatchEditModel>
    {
        public SpotBatchEditModelValidator()
        {
            RuleFor(x => x.ZoneId)
                .GreaterThan(0);

            RuleFor(x => x.Name)
                 .NotEmpty()
                 .Length(1, 100);
        }
    }
}
