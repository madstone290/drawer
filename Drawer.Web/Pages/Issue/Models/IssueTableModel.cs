namespace Drawer.Web.Pages.Issue.Models
{
    public class IssueTableModel
    {
        public long Id { get; set; }
        public string? TransactionNumber { get; set; } = Guid.NewGuid().ToString();
        public string? IssueDateString { get; set; }
        public string? IssueTimeString { get; set; }
        public long ItemId { get; set; }
        public string? ItemName { get; set; }
        public long LocationId { get; set; }
        public string? LocationName { get; set; }
        public string? QuantityString { get; set; }
        public string? Buyer { get; set; }
        public string? Note { get; set; }
    }
}
