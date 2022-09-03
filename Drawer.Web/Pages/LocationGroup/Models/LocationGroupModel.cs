using FluentValidation;

namespace Drawer.Web.Pages.LocationGroup.Models
{
    public class LocationGroupModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Note { get; set; }
        public long RootGroupId { get; set; }
        public long ParentGroupId { get; set; }
        public string? ParentGroupName { get; set; }
        public int Depth { get; set; }
    }

    public class LocationGroupModelValidator : AbstractValidator<LocationGroupModel>
    {
        /// <summary>
        /// 위치명 목록
        /// </summary>
        public List<string>? GroupNameList { get; set; }

        public LocationGroupModelValidator()
        {
            RuleFor(x => x.Name)
                 .NotEmpty()
                 .WithMessage("* 필수");

            RuleFor(x => x.ParentGroupName)
                .Custom((value, context) =>
                {
                    if(GroupNameList == null)
                    {
                        context.AddFailure("그룹목록이 없습니다");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(value))
                        return;
                    if (GroupNameList.Any(name => string.Equals(name, value, StringComparison.OrdinalIgnoreCase)))
                        return;
                    else
                        context.AddFailure("그룹목록에 없는 위치입니다");
                });
        }
    }
}
