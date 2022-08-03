using FluentValidation;

namespace Drawer.Web.Pages.LocationOld.Models
{
    public class ZoneModel
    {
        public long Id { get; set; }
        public long WorkplaceId { get; set; }
        public string? WorkplaceName { get; set; }
        public string? Name { get; set; }
        public string? Note { get; set; }
    }

    public class ZoneModelValidator : AbstractValidator<ZoneModel>
    {
        public ZoneModelValidator()
        {
            RuleFor(x => x.WorkplaceId)
                .GreaterThan(0).WithMessage("* 필수");

            RuleFor(x => x.Name)
                 .NotEmpty().WithMessage("* 필수");
        }
    }
}
