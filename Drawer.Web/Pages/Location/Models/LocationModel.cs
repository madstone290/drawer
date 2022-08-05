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
        /// <summary>
        /// 위치명 목록
        /// </summary>
        public List<string>? LocationNames { get; set; }

        public LocationModelValidator()
        {
            RuleFor(x => x.Name)
                 .NotEmpty()
                 .WithMessage("* 필수");

            RuleFor(x => x.UpperLocationName)
                .Custom((value, context) =>
                {
                    if(LocationNames == null)
                    {
                        context.AddFailure("이름목록이 없습니다");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(value))
                        return;
                    if (LocationNames.Any(name => string.Equals(name, value, StringComparison.OrdinalIgnoreCase)))
                        return;
                    else
                        context.AddFailure("목록에 없는 위치입니다");
                });
        }
    }
}
