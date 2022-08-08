using Drawer.Domain.Config;
using Drawer.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models.Inventory
{
    /// <summary>
    /// 입고
    /// </summary>
    public class Receipt : CompanyResourceEntity<long>
    {
        public Receipt(long itemId, long locationId, decimal quantity)
        {
            SetInventoryInfo(itemId, locationId, quantity);
            SetReceiptTime(DateTime.UtcNow);
        }

        /// <summary>
        /// 아이템
        /// </summary>
        public long ItemId { get; private set; }

        /// <summary>
        /// 아이템을 보관할 위치
        /// </summary>
        public long LocationId { get; private set; }

        public decimal Quantity { get; private set; }

        /// <summary>
        /// 입고시간(UTC)
        /// </summary>
        public DateTime ReceiptTime { get; private set; }

        /// <summary>
        /// 입고시간(로컬)
        /// </summary>
        public DateTime ReceiptTimeLocal => ReceiptTime.ToLocalTime();

        /// <summary>
        /// 판매자
        /// </summary>
        public string? Seller { get; private set; }

        /// <summary>
        /// 아이템, 위치, 수량을 변경한다.
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="locationId"></param>
        /// <param name="quantity"></param>
        public void SetInventoryInfo(long itemId, long locationId, decimal quantity)
        {
            ItemId = itemId;
            LocationId = locationId;
            SetQuantity(quantity);
        }

        /// <summary>
        /// 수량을 변경한다.
        /// </summary>
        /// <param name="quantity"></param>
        /// <exception cref="DomainException"></exception>
        public void SetQuantity(decimal quantity)
        {
            if (quantity <= 0)
                throw new DomainException("입고수량은 0보다 커야합니다");
            Quantity = quantity;
        }


        public void SetReceiptTime(DateTime receiptTime)
        {
            ReceiptTime = receiptTime.ToUniversalTime();
        }

        public void SetSeller(string? seller)
        {
            Seller = seller;
        }







    }
}
