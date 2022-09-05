using FluentValidation;

namespace Drawer.Web.Pages.Location.Models
{
    public class LocationModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Note { get; set; }
        public long GroupId { get; set; }
        public string? GroupName { get; set; }
    }

    public class LocationModelValidator : AbstractValidator<LocationModel>
    {
        /// <summary>
        /// 그룹명 목록
        /// </summary>
        public List<string>? GroupNames { get; set; }

        public LocationModelValidator()
        {
            RuleFor(x => x.Name)
                 .NotEmpty()
                 .WithMessage("* 필수");

            RuleFor(x => x.GroupName)
                .Custom((value, context) =>
                {
                    if(GroupNames == null)
                    {
                        context.AddFailure("그룹목록이 없습니다");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(value))
                        return;
                    if (GroupNames.Any(name => string.Equals(name, value, StringComparison.OrdinalIgnoreCase)))
                        return;
                    else
                        context.AddFailure("목록에 없는 그룹입니다");
                });
        }
    }
}
