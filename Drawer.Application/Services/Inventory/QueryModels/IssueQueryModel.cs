using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.QueryModels
{
    /// <summary>
    /// 출고내역
    /// </summary>
    public class IssueQueryModel
    {
        public long Id { get; set; }
        public string TransactionNumber { get; set; } = null!;
        public DateTime IssueDateTimeUtc { get; set; }
        public DateTime IssueDateTimeLocal => IssueDateTimeUtc.ToLocalTime();
        public long ItemId { get; set; }
        public long LocationId { get; set; }
        public decimal Quantity { get; set; }
        public string? Buyer { get; set; }
        public string? Note { get; set; }



    }
}
