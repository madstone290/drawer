using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Contract.Items
{
    public record UpdateItemRequest(string Name, string? Code, string? Number,
        string? Sku, string? MeasurementUnit);
}
