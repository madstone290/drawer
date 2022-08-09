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
    /// 출고
    /// </summary>
    public class Issue : CompanyResourceEntity<long>
    {
        public Issue(string transactionNumber, long itemId, long locationId, decimal quantity)
        {
            TransactionNumber = transactionNumber;
            SetInventoryInfo(itemId, locationId, quantity);
            SetIssueTime(DateTime.UtcNow);
        }
        
        /// <summary>
        /// 출고번호
        /// </summary>
        public string TransactionNumber { get; private set; }

        /// <summary>
        /// 출고시간(Utc)
        /// </summary>
        public DateTime IssueDateTime { get; private set; }

        /// <summary>
        /// 출고시간(Local)
        /// </summary>
        public DateTime IssueDateTimeLocal => IssueDateTime.ToLocalTime();

        public long ItemId { get; private set; }

        /// <summary>
        /// 아이템이 보관되던 위치
        /// </summary>
        public long LocationId { get; private set; }

        /// <summary>
        /// 출고수량
        /// </summary>
        public decimal Quantity { get; private set; }

        /// <summary>
        /// 구매자
        /// </summary>
        public string? Buyer { get; private set; }

        /// <summary>
        /// 아이템, 위치, 수량을 변경한다.
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="locationㅑㅇ"></param>
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
                throw new DomainException("출고수량은 0보다 커야합니다");
            Quantity = quantity;
        }

        public void SetIssueTime(DateTime issueTime)
        {
            IssueDateTime = issueTime.ToUniversalTime();
        }

        public void SetBuyer(string? buyer)
        {
            Buyer = buyer;
        }
    }
}
