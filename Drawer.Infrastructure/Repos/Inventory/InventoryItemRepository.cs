using Drawer.Application.Services.Inventory.QueryModels;
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

        public async Task<InventoryItem?> FindByItemIdAndLocationIdAsync(long itemId, long locationId)
        {
            return await _dbContext.InventoryItems.FirstOrDefaultAsync(x => x.ItemId == itemId && x.LocationId == locationId);
        }

        public async Task<InventoryItemQueryModel?> QueryByItemIdAndLocationId(long itemId, long locationId)
        {
            return await _dbContext.InventoryItems.Where(x => x.ItemId == itemId && x.LocationId == locationId)
                  .SelectQueryModel()
                  .FirstOrDefaultAsync();
        }

        public async Task<List<InventoryItemQueryModel>> QueryAll()
        {
            return await _dbContext.InventoryItems
                .SelectQueryModel()
                .ToListAsync();
        }

        public async Task<List<InventoryItemQueryModel>> QueryByItemId(long itemId)
        {
            return await _dbContext.InventoryItems
                .Where(x => x.ItemId == itemId)
                .SelectQueryModel()
                .ToListAsync();
        }

        public async Task<List<InventoryItemQueryModel>> QueryByLocationId(long locationId)
        {
            return await _dbContext.InventoryItems.Where(x => x.LocationId == locationId)
               .SelectQueryModel()
               .ToListAsync();
        }

    }

    public static class InventoryItemRepositoryExtensions
    {
        public static IQueryable<InventoryItemQueryModel> SelectQueryModel(this IQueryable<InventoryItem> query)
        {
            return query.Select(x => new InventoryItemQueryModel()
            {
                ItemId = x.ItemId,
                LocationId = x.LocationId,
                Quantity = x.Quantity
            });
        }
    }

}

