using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.QueryModels
{
    /// <summary>
    /// 입고내역
    /// </summary>
    public class ReceiptQueryModel
    {
        public ReceiptQueryModel() { }
        public ReceiptQueryModel(long id, string transactionNumber, DateTime receiptDateTime, long itemId, long locationId, decimal quantity, string? seller)
        {
            Id = id;
            TransactionNumber = transactionNumber;
            ReceiptDateTimeUtc = receiptDateTime;
            ItemId = itemId;
            LocationId = locationId;
            Quantity = quantity;
            Seller = seller;
        }

        /// <summary>
        /// 아이디
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 입고번호
        /// </summary>
        public string TransactionNumber { get; set; } = null!;

        /// <summary>
        /// 입고일시(UTC)
        /// </summary>
        public DateTime ReceiptDateTimeUtc { get; set; }

        /// <summary>
        /// 입고일시(로컬)
        /// </summary>
        public DateTime ReceiptDateTimeLocal => ReceiptDateTimeUtc.ToLocalTime();

        /// <summary>
        /// 아이템
        /// </summary>
        public long ItemId { get; set; }

        /// <summary>
        /// 아이템을 보관할 위치
        /// </summary>
        public long LocationId { get; set; }

        /// <summary>
        /// 입고수량
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 판매자
        /// </summary>
        public string? Seller { get; set; }



    }
}
