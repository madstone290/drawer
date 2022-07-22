using FluentValidation;

namespace Drawer.Web.Pages.Items.Models
{
    public class ItemModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string QuantityUnit { get; set; } = string.Empty;
    }

    public class ItemModelValidator : AbstractValidator<ItemModel>
    {
        public ItemModelValidator()
        {
            RuleFor(x => x.Name)
                 .NotEmpty()
                 .Length(1, 100);
        }
    }
}
