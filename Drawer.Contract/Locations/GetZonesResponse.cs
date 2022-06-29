using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Drawer.Contract.Locations.GetZonesResponse;

namespace Drawer.Contract.Locations
{
    public record GetZonesResponse(IList<Zone> Zones)
    {
        public record Zone(long Id, string Name, long? ZoneTypeId);
    }
}
