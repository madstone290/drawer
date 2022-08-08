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
            return await _dbContext.Items.AnyAsync(x=> x.Name == name);
        }

        public async Task<IList<Item>> FindAll()
        {
            return await _dbContext.Items.ToListAsync();
        }
    }
}
