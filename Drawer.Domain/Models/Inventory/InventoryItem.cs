using Drawer.Domain.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models.Inventory
{
    /// <summary>
    /// 재고 아이템. 아이템/위치의 수량정보를 가진다.
    /// </summary>
    public class InventoryItem : CompanyResourceEntity<long>
    {
        /// <summary>
        /// 마스터 아이템
        /// </summary>
        public long ItemId { get; private set; }

        /// <summary>
        /// 재고를 보관할 위치
        /// </summary>
        public long LocationId { get; private set; }

        /// <summary>
        /// 재고 수량
        /// </summary>
        public decimal Quantity { get; private set; }

        private InventoryItem() { }
        public InventoryItem(long itemId, long locationId, decimal quantity)
        {
            ItemId = itemId;
            LocationId = locationId;
            Increase(quantity);
        }

        /// <summary>
        /// 재고수량을 설정한다.
        /// </summary>
        /// <param name="quantity"></param>
        public void SetQuantity(decimal quantity)
        {
            var quantityChange = quantity - Quantity;
            Add(quantityChange);
        }

        /// <summary>
        /// 재고수량을 더한다.
        /// 양수인경우 증가, 음수인 경우 감소.
        /// </summary>
        /// <param name="quantity">재고 변경수량</param>
        public void Add(decimal quantity)
        {
            if (0 < quantity)
                Increase(quantity);
            else
                Decrease(-1 * quantity);
        }

        /// <summary>
        /// 재고수량을 뺀다.
        /// 양수인 경우 감소, 음수인 경우 증가.
        /// </summary>
        /// <param name="quantity">재고 변경수량</param>
        public void Minus(decimal quantity)
        {
            if (0 < quantity)
                Decrease(quantity);
            else
                Increase(-1 * quantity);
        }

        public void Increase(decimal quantity)
        {
            if (quantity < 0)
                throw new DomainException("수량은 음수일 수 없습니다");
            Quantity += quantity;
        }

        public void Decrease(decimal quantity)
        {
            if (quantity < 0)
                throw new DomainException("수량은 음수일 수 없습니다");
            if (Quantity < quantity)
                throw new DomainException("감소수량이 재고수량보다 많습니다");
                
            Quantity -= quantity;
        }
    }
}
