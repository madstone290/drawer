using Drawer.Application.Services.Items.Repos;
using Drawer.Domain.Models.Items;
using Drawer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Infrastructure.Repos.Items
{
    public class ItemRepository : Repository<Item, long>, IItemRepository
    {
        public ItemRepository(DrawerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IList<Item>> FindAll()
        {
            return await _dbContext.Items.ToListAsync();
        }
    }
}
