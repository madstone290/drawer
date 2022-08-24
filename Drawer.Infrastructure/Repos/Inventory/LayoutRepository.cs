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
    public class LayoutRepository : Repository<Layout, long>, ILayoutRepository
    {
        public LayoutRepository(DrawerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<LayoutQueryModel>> QueryAll()
        {
            return await _dbContext.Layouts
                .Include(x => x.Items)
                .SelectQueryModel()
                .ToListAsync();
        }

        public async Task<LayoutQueryModel?> QueryById(long id)
        {
            return await _dbContext.Layouts
                .Where(x=> x.Id == id)
                .Include(x=> x.Items)
                .SelectQueryModel()
                .FirstOrDefaultAsync();
        }
    }

    public static class LayoutRepositoryExtensions
    {
        public static IQueryable<LayoutQueryModel> SelectQueryModel(this IQueryable<Layout> query)
        {
            return query.AsNoTracking().Select(x => new LayoutQueryModel()
            {
                Id = x.Id,
                LocationId = x.LocationId,
                ItemList = x.Items.ToList()
            });
        }
    }
}
