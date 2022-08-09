namespace Drawer.Web.Pages.InventoryStatus.Models
{
    /// <summary>
    /// 아이템 및 수량
    /// </summary>
    public class ItemInventoryModel
    {
        public long ItemId { get; set; }
        public string? ItemName { get; set; }
        public decimal Quantity { get; set; }
    }
}
