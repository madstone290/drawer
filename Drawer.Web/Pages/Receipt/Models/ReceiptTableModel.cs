namespace Drawer.Web.Pages.Receipt.Models
{
    public class ReceiptTableModel
    {
        public long Id { get; set; }
        public string? TransactionNumber { get; set; } = Guid.NewGuid().ToString();
        public string? ReceiptDateString { get; set; }
        public string? ReceiptTimeString { get; set; }
        public long ItemId { get; set; }
        public string? ItemName { get; set; }
        public long LocationId { get; set; }
        public string? LocationName { get; set; }
        public string? QuantityString { get; set; }
        public string? Seller { get; set; }
        public string? Note { get; set; }
    }
}
