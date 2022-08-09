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
    public class ReceiptRepository : Repository<Receipt, long>, IReceiptRepository
    {
        public ReceiptRepository(DrawerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Receipt>> FindByReceiptDateBetween(DateTime from, DateTime to)
        {
            var utcTimeFrom = from.Date.ToUniversalTime();
            var utcTimeTo = to.Date.AddDays(1).AddTicks(-1).ToUniversalTime();
            return await _dbContext.Receipts
                .Where(x => utcTimeFrom <= x.ReceiptDateTime && x.ReceiptDateTime <= utcTimeTo)
                .ToListAsync();
        }
    }
}
