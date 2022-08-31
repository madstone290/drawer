﻿using Drawer.Application.Services.Inventory.QueryModels;
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
    public class LocationRepository : Repository<Location, long>, ILocationRepository
    {
        public LocationRepository(DrawerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> ExistByName(string name)
        {
            return await _dbContext.Locations.AnyAsync(x => x.Name == name);
        }

        public async Task<bool> ExistByUpperLocationId(long locationId)
        {
            return await _dbContext.Locations.AnyAsync(x => x.ParentGroupId == locationId);
        }

        public async Task<List<LocationQueryModel>> QueryAll()
        {
            return await _dbContext.Locations
                .SelectQueryModel()
                .ToListAsync();
        }

        public async Task<LocationQueryModel?> QueryById(long id)
        {
            return await _dbContext.Locations
                .Where(x => x.Id == id)
                .SelectQueryModel()
                .FirstOrDefaultAsync();
        }
    }

    public static class LocationRepositoryExtensions
    {
        public static IQueryable<LocationQueryModel> SelectQueryModel(this IQueryable<Location> query)
        {
            return query.Select(x => new LocationQueryModel()
            {
                Id = x.Id,
                Name = x.Name,
                Note = x.Note,
                IsGroup = x.IsGroup,
                ParentGroupId = x.ParentGroupId,
                HierarchyLevel = x.HierarchyLevel,
                RootGroupId = x.ActualRootGroupId,
                IsRootGroup = x.IsRootGroup
            });
        }
    }
}
