using Drawer.Application.Services.Inventory.QueryModels;
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

        Task<InventoryItemQueryModel?> QueryByItemIdAndLocationId(long itemId, long locationId);

        Task<List<InventoryItemQueryModel>> QueryAll();

        Task<List<InventoryItemQueryModel>> QueryByItemId(long itemId);

        Task<List<InventoryItemQueryModel>> QueryByLocationId(long locationId);
    }
}
