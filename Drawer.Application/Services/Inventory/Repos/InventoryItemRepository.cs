using Drawer.Domain.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.Repos
{
    public interface IInventoryItemRepository : IRepository<InventoryItem>
    {
        Task<InventoryItem?> FindByItemIdAndLocationIdAsync(long itemId, long locationId);

        Task<IList<InventoryItem>> FindByItemIdAsync(long itemId);

        Task<IList<InventoryItem>> FindByLocationIdAsync(long locationId);

        Task<IList<InventoryItem>> FindAll();
     
    }
}
