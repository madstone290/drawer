namespace Drawer.Web.Pages.Inventory.Models
{
    /// <summary>
    /// 아이템, 위치 및 수량
    /// </summary>
    public class ItemLocationInventoryModel
    {
        public long ItemId { get; set; }
        public string? ItemName { get; set; }
        public long LocationId { get; set; }
        public string? LocationName { get; set; }
        public decimal Quantity { get; set; }
    }
}
