using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Drawer.Contract.Locations.GetZonesResponse;

namespace Drawer.Contract.Locations
{
    public class ZoneContracts
    {
    }

    public record BatchCreateZoneRequest(IList<BatchCreateZoneRequest.Zone> Zones)
    {
        public record Zone(long WorkplaceId, string Name, string? Note);
    }

    public record BatchCreateZoneResponse(IList<long> IdList);

    public record CreateZoneRequest(long WorkplaceId, string Name, string? Note);

    public record CreateZoneResponse(long Id);

    public record UpdateZoneRequest(string Name, string? Note);

    public record GetZoneResponse(long Id, long WorkplaceId, string Name, string? Note);

    public record GetZonesResponse(IList<Zone> Zones)
    {
        public record Zone(long Id, long WorkplaceId, string Name, string? Note);
    }
}
