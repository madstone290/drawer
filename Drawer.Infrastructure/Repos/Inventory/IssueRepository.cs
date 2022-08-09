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
    public class IssueRepository : Repository<Issue, long>, IIssueRepository
    {
        public IssueRepository(DrawerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Issue>> FindByIssueDateBetween(DateTime from, DateTime to)
        {
            var utcTimeFrom = from.Date.ToUniversalTime();
            var utcTimeTo = to.Date.AddDays(1).AddTicks(-1).ToUniversalTime();
            return await _dbContext.Issues
                .Where(x => utcTimeFrom <= x.IssueDateTime && x.IssueDateTime <= utcTimeTo)
                .ToListAsync();
        }
    }
}
