using Drawer.Application.Services.BasicInfo.Repos;
using Drawer.Domain.Models.BasicInfo;
using Drawer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Infrastructure.Repos.Locations
{
    public class LocationRepository : Repository<Location, long>, ILocationRepository
    {
        public LocationRepository(DrawerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IList<Location>> FindAll()
        {
            return await _dbContext.Locations.ToListAsync();
        }

    }
}
