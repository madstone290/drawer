using Drawer.Application.Services.Locations.Repos;
using Drawer.Domain.Models.Locations;
using Drawer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Infrastructure.Repos.Locations
{
    public class WorkPlaceRepository : Repository<Workplace, long>, IWorkplaceRepository
    {
        public WorkPlaceRepository(DrawerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IList<Workplace>> FindAll()
        {
            return await _dbContext.WorkPlaces.ToListAsync();
        }

    }
}
