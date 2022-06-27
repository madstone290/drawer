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
    public class WorkPlaceRepository : Repository<WorkPlace>, IWorkPlaceRepository
    {
        public WorkPlaceRepository(DrawerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<WorkPlace> FilterTest(string value)
        {
            return await _dbContext.WorkPlaces.FirstOrDefaultAsync(x => x.Name == value);
        }
    }
}
