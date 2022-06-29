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
    public class PositionRepository : Repository<Position, long>, IPositionRepository
    {
        public PositionRepository(DrawerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IList<Position>> FindAll()
        {
            return await _dbContext.Positions.ToListAsync();
        }

    }
}
