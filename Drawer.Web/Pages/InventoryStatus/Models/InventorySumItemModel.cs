namespace Drawer.Web.Pages.InventoryStatus.Models
{
    /// <summary>
    /// 수량합계 재고아이템 모델.
    /// </summary>
    public class InventorySumItemModel
    {
        public long ItemId { get; set; }
        public string? ItemName { get; set; }
        public decimal Quantity { get; set; }
    }
}
