using Drawer.Shared;
using FluentValidation;

namespace Drawer.Web.Pages.Issue.Models
{
    public class IssueModel
    {
        public IssueModel()
        {
            IssueDate = DateTimeExtensions.KoreaToday;
            IssueTime = DateTimeExtensions.KoreaNowTime;

            IssueDateString = IssueDate.Value.ToString("yyyy-MM-dd");
            IssueTimeString = IssueTime.Value.ToString(@"hh\:mm");
        }

        public long Id { get; set; }
        public string? TransactionNumber { get; set; }
        public DateTime? IssueDate { get; set; } 
        public TimeSpan? IssueTime { get; set; } 
        public DateTime IssueDateTime
        {
            get
            {
                if (IssueDate == null || IssueTime == null)
                    throw new Exception("Date or Time is null");
                return IssueDate.Value.Add(IssueTime.Value);
            }
        }
        public string? IssueDateString { get; set; }
        public string? IssueTimeString { get; set; }

        public long ItemId { get; set; }
        public string? ItemName { get; set; }
        public long LocationId { get; set; }
        public string? LocationName { get; set; }
        public decimal Quantity { get; set; }
        public string? Buyer { get; set; }
        public string? Note { get; set; }


    }

    public class IssueModelValidator : AbstractValidator<IssueModel>
    {
        public List<string>? ItemNames { get; set; }

        public List<string>? LocationNames { get; set; }

        public IssueModelValidator()
        {
            RuleFor(x => x.IssueDate)
                .NotEmpty()
                .WithMessage("필수 항목입니다");

            RuleFor(x => x.IssueTime)
                .NotEmpty()
                .WithMessage("필수 항목입니다");

            RuleFor(x => x.ItemId)
                 .GreaterThan(0)
                 .WithMessage("필수 항목입니다");

            RuleFor(x => x.ItemName)
                .Custom((value, context) =>
                {
                    if (ItemNames == null)
                    {
                        context.AddFailure("아이템 목록이 없습니다");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(value))
                    {
                        context.AddFailure("필수 항목입니다");
                        return;
                    }

                    if (ItemNames.Any(name => string.Equals(name, value, StringComparison.OrdinalIgnoreCase)) == false)
                    {
                        context.AddFailure("아이템이 유효하지 않습니다");
                        return;
                    }
                });

            RuleFor(x => x.LocationId)
                .GreaterThan(0)
                .WithMessage("필수 항목입니다");

            RuleFor(x => x.LocationName)
                .Custom((value, context) =>
                {
                    if (LocationNames == null)
                    {
                        context.AddFailure("위치 목록이 없습니다");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(value))
                    {
                        context.AddFailure("필수 항목입니다");
                        return;
                    }

                    if (LocationNames.Any(name => string.Equals(name, value, StringComparison.OrdinalIgnoreCase)) == false)
                    {
                        context.AddFailure("위치가 유효하지 않습니다");
                        return;
                    }
                });

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("0보다 커야합니다");
        }
    }
}
