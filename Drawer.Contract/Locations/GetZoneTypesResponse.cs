using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Drawer.Contract.Locations.GetZoneTypesResponse;

namespace Drawer.Contract.Locations
{
    public record GetZoneTypesResponse(IList<ZoneType> ZoneTypes)
    {
        public record ZoneType(long Id, string Name);
    }
}
