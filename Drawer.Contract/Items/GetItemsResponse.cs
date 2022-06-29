using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Drawer.Contract.Items.GetItemsResponse;

namespace Drawer.Contract.Items
{
    public record GetItemsResponse(IList<Item> Items)
    {
        public record Item(long Id, string Name, string? Code, string? Number,
             string? Sku, string? MeasurementUnit);
    }
}
