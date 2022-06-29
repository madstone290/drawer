using Drawer.Domain.Models.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Locations.Repos
{
    public interface IZoneTypeRepository : IRepository<ZoneType, long>
    {
        Task<IList<ZoneType>> FindAll(); 
    }
}
