using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.QueryModels
{
    public class LocationQueryModel
    {
        public long Id { get; set; }
        public long RootGroupId { get; set; }
        public long? ParentGroupId { get; set; }
        public string Name { get; set; } = null!;
        public string? Note { get; set; }
        public bool IsGroup { get; set; }
        public bool IsRootGroup { get; set; }
        public int HierarchyLevel { get; set; }
        
    }
}
