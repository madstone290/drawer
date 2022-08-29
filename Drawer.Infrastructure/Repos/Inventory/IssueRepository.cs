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
    public class IssueRepository : Repository<Issue, long>, IIssueRepository
    {
        public IssueRepository(DrawerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IssueQueryModel?> QueryById(long id)
        {
            return await _dbContext.Issues
              .Where(x => x.Id == id)
              .SelectQueryModel()
              .FirstOrDefaultAsync();
        }

        public async Task<List<IssueQueryModel>> QueryByIssueDateBetween(DateTime from, DateTime to)
        {
            var utcTimeFrom = from.Date.ToUniversalTime();
            var utcTimeTo = to.Date.AddDays(1).AddTicks(-1).ToUniversalTime();
            return await _dbContext.Issues
                .Where(x => utcTimeFrom <= x.IssueDateTime && x.IssueDateTime <= utcTimeTo)
                .OrderBy(x => x.IssueDateTime)
                .SelectQueryModel()
                .ToListAsync();
        }
    }

    public static class IssueRepositoryExtensions
    {
        public static IQueryable<IssueQueryModel> SelectQueryModel(this IQueryable<Issue> query)
        {
            return query.Select(x => new IssueQueryModel()
            {
                Id = x.Id,
                TransactionNumber = x.TransactionNumber,
                IssueDateTimeUtc = x.IssueDateTime,
                ItemId = x.ItemId,
                LocationId = x.LocationId,
                Quantity = x.Quantity,
                Buyer = x.Buyer,
                Note = x.Note
            });
        }
    }
}
