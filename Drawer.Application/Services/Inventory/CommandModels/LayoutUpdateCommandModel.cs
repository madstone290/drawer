using Drawer.Domain.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.CommandModels
{
    public class LayoutUpdateCommandModel
    {
        public List<LayoutItem> ItemList { get; set; } = new List<LayoutItem>();
    }
}
