using Drawer.Domain.Config;
using Drawer.Domain.Models.BasicInfo;
using Drawer.Domain.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models.InventoryManagement
{
    public class InventoryDetail : CompanyResourceEntity<long>
    {
        public Item Item { get; private set; } = null!;
        public long ItemId { get; private set; }

        public Location Location { get; private set; } = null!;
        public long LocationId { get; private set; }

        public decimal Quantity { get; private set; }

        private InventoryDetail() { }
        public InventoryDetail(Item item, Location location, decimal quantity)
        {
            Item = item;
            Location = location;
            Increase(quantity);
        }

        /// <summary>
        /// 재고수량을 설정한다.
        /// </summary>
        /// <param name="quantity"></param>
        public void SetQuantity(decimal quantity)
        {
            var quantityChange = quantity - Quantity;
            Change(quantityChange);
        }

        /// <summary>
        /// 재고수량을 변경한다.
        /// </summary>
        /// <param name="quantityChange">재고 변경수량</param>
        public void Change(decimal quantityChange)
        {
            if (0 < quantityChange)
                Increase(quantityChange);
            else
                Decrease(-1 * quantityChange);
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
