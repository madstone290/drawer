using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.CommandModels
{
    public class ReceiptAddUpdateCommandModel
    {
        public DateTime ReceiptDateTime { get; set; }
        public long ItemId { get; set; }
        public long LocationId { get; set; }
        public decimal Quantity { get; set; }
        public string? Seller { get; set; }
    }
}
