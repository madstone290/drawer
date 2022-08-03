using Drawer.Web.DataBinding;
using FluentValidation;

namespace Drawer.Web.Pages.LocationOld.Models
{
    public class SpotModel
    { 
        public long Id { get; set; }
        public string? Name { get; set; } 
        public string? Note { get; set; } 
        public long ZoneId { get; set; }
        public string? ZoneName { get; set; }
    }

    public class SpotModelValidator : AbstractValidator<SpotModel>
    {
        public SpotModelValidator()
        {
            RuleFor(x => x.ZoneId)
                .GreaterThan(0)
                .WithMessage("* 필수");


            RuleFor(x => x.Name)
                 .NotEmpty()
                 .WithMessage("* 필수");
                 
        }
    }
}
