using Drawer.Application.Services.InventoryManagement.Repos;
using Drawer.Domain.Models.InventoryManagement;
using Drawer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Infrastructure.Repos.InventoryManagement
{
    public class InventoryDetailRepository : Repository<InventoryDetail, long>, IInventoryDetailRepository
    {
        public InventoryDetailRepository(DrawerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IList<InventoryDetail>> FindAll()
        {
            return await _dbContext.InventoryDetails.ToListAsync();
        }

        public async Task<InventoryDetail?> FindByItemIdAndLocationIdAsync(long itemId, long locationId)
        {
            return await _dbContext.InventoryDetails.FirstOrDefaultAsync(x=> x.ItemId == itemId && x.LocationId == locationId);
        }

        public async Task<IList<InventoryDetail>> FindByItemIdAsync(long itemId)
        {
            return await _dbContext.InventoryDetails.Where(x=> x.ItemId == itemId).ToListAsync();
        }

        public async Task<IList<InventoryDetail>> FindByLocationIdAsync(long locationId)
        {
            return await _dbContext.InventoryDetails.Where(x=> x.LocationId == locationId).ToListAsync();
        }
    }
}
