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
    public class ItemRepository : Repository<Item, long>, IItemRepository
    {
        public ItemRepository(DrawerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> ExistByName(string name)
        {
            return await _dbContext.Items.AnyAsync(x => x.Name == name);
        }

        public async Task<List<ItemQueryModel>> QueryAll()
        {
            return await _dbContext.Items
                .Select(x => new ItemQueryModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Number = x.Number,
                    Sku = x.Sku,
                    QuantityUnit = x.QuantityUnit

                })
                .ToListAsync();
        }

        public async Task<ItemQueryModel?> QueryById(long id)
        {
            return await _dbContext.Items
                .Where(x => x.Id == id)
                .Select(x => new ItemQueryModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Number = x.Number,
                    Sku = x.Sku,
                    QuantityUnit = x.QuantityUnit

                })
                .FirstOrDefaultAsync();
        }

    }
}
