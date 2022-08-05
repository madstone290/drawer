using FluentValidation;

namespace Drawer.Web.Pages.Item.Models
{
    public class ItemModel
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Number { get; set; }
        public string? Sku { get; set; }
        public string? QuantityUnit { get; set; }
    }

    public class ItemModelValidator : AbstractValidator<ItemModel>
    {
        public ItemModelValidator()
        {
            RuleFor(x => x.Name)
                 .NotEmpty()
                 .WithMessage("* 필수");
                 
        }
    }
}
