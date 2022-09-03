using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.CommandModels
{
    public class LocationGroupUpdateCommandModel
    {
        public string Name { get; set; } = default!;
        public string? Note { get; set; }
    }
}
