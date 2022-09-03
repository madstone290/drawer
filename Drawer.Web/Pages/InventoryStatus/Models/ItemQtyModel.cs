namespace Drawer.Web.Pages.InventoryStatus.Models
{
    /// <summary>
    /// 재고수량 모델.
    /// </summary>
    public class ItemQtyModel
    {
        public long ItemId { get; set; }
        public string? ItemName { get; set; }
        public decimal Quantity { get; set; }
    }
}
