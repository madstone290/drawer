namespace Drawer.Web.Pages.Item.Models
{
    public class ItemTableModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string QuantityUnit { get; set; } = string.Empty;
    }
}
