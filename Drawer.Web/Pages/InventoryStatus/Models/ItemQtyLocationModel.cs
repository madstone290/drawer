namespace Drawer.Web.Pages.InventoryStatus.Models
{
    /// <summary>
    /// 재고아이템 기본 모델
    /// </summary>
    public class ItemQtyLocationModel
    {
        public long ItemId { get; set; }
        public string? ItemName { get; set; }
        public long LocationId { get; set; }
        public string? LocationName { get; set; }
        public decimal Quantity { get; set; }
    }
}
