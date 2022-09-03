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
    public class LocationGroupRepository : Repository<LocationGroup, long>, ILocationGroupRepository
    {
        public LocationGroupRepository(DrawerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> ExistByName(string name)
        {
            return await _dbContext.LocationGroups.AnyAsync(x => x.Name == name);
        }

        public async Task<bool> ExistByParentGroup(long parentGroupId)
        {
            return await _dbContext.LocationGroups.AnyAsync(x => x.ParentGroupId == parentGroupId);
        }

        public async Task<List<LocationGroupQueryModel>> QueryAll()
        {
            return await _dbContext.LocationGroups
                .SelectQueryModel()
                .ToListAsync();
        }

        public async Task<LocationGroupQueryModel?> QueryById(long id)
        {
            return await _dbContext.LocationGroups
                .Where(x => x.Id == id)
                .SelectQueryModel()
                .FirstOrDefaultAsync();
        }
    }

    public static class LocationGroupRepositoryExtensions
    {
        public static IQueryable<LocationGroupQueryModel> SelectQueryModel(this IQueryable<LocationGroup> query)
        {
            return query.Select(x => new LocationGroupQueryModel()
            {
                Id = x.Id,
                RootGroupId = x.RootGroupId,
                ParentGroupId = x.ParentGroupId,
                Name = x.Name,
                Note = x.Note,
                Depth = x.Depth,
                IsRoot = x.IsRoot,
            });
        }
    }
}
