using FluentValidation;

namespace Drawer.Web.Pages.Receipt.Models
{
    public class ReceiptModel
    {
        public ReceiptModel()
        {
            ReceiptDateString = DateTime.Today.ToString("yyyy-MM-dd");
            ReceiptTimeString = DateTime.Now.TimeOfDay.ToString(@"hh\:mm");
        }

        public long Id { get; set; }
        public string? TransactionNumber { get; set; } = Guid.NewGuid().ToString();
        public DateTime? ReceiptDate { get; set; } = DateTime.Now.Date;
        public TimeSpan? ReceiptTime { get; set; } = DateTime.Now.TimeOfDay;
        public DateTime ReceiptDateTime
        {
            get
            {
                if (ReceiptDate == null || ReceiptTime == null)
                    throw new Exception("Date or Time is null");
                return ReceiptDate.Value.Add(ReceiptTime.Value);
            }
        }
        public string? ReceiptDateString { get; set; }
        public string? ReceiptTimeString { get; set; }

        public long ItemId { get; set; }
        public string? ItemName { get; set; }
        public long LocationId { get; set; }
        public string? LocationName { get; set; }
        public decimal Quantity { get; set; }
        public string? Seller { get; set; }
        public string? Note { get; set; }
    }

    public class ReceiptModelValidator : AbstractValidator<ReceiptModel>
    {
        public List<string>? ItemNames { get; set; }

        public List<string>? LocationNames { get; set; }

        public ReceiptModelValidator()
        {
            RuleFor(x => x.ReceiptDate)
                .NotEmpty()
                .WithMessage("필수 항목입니다");

            RuleFor(x => x.ReceiptTime)
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

            RuleFor(x => x.ReceiptDateString)
              .Custom((value, context) =>
              {
                  if (!DateTime.TryParse(value, out var time))
                      context.AddFailure("유효한 날짜형식이 아닙니다");
              });

            RuleFor(x=> x.ReceiptTimeString)
                .Custom((value, context) =>
                {
                    if (!TimeSpan.TryParse(value, out var time))
                        context.AddFailure("유효한 시간형식이 아닙니다");
                });
        }
    }
}
