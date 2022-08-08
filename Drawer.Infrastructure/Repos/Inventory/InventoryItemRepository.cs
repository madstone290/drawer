using Drawer.Application.Services.Inventory.Repos;
using Drawer.Domain.Models.Inventory;
using Drawer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Infrastructure.Repos.Inventory
{
    public class InventoryItemRepository : Repository<InventoryItem, long>, IInventoryItemRepository
    {
        public InventoryItemRepository(DrawerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IList<InventoryItem>> FindAll()
        {
            return await _dbContext.InventoryItems.ToListAsync();
        }

        public async Task<InventoryItem?> FindByItemIdAndLocationIdAsync(long itemId, long locationId)
        {
            return await _dbContext.InventoryItems.FirstOrDefaultAsync(x=> x.ItemId == itemId && x.LocationId == locationId);
        }

        public async Task<IList<InventoryItem>> FindByItemIdAsync(long itemId)
        {
            return await _dbContext.InventoryItems.Where(x=> x.ItemId == itemId).ToListAsync();
        }

        public async Task<IList<InventoryItem>> FindByLocationIdAsync(long locationId)
        {
            return await _dbContext.InventoryItems.Where(x=> x.LocationId == locationId).ToListAsync();
        }
    }
}
