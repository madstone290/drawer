using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.QueryModels
{
    public class InventoryItemQueryModel
    {
        public long ItemId { get; set; }
        public long LocationId { get; set; }
        public decimal Quantity { get; set; }
    }
}
