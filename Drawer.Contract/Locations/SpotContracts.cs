using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Drawer.Contract.Locations.GetSpotsResponse;

namespace Drawer.Contract.Locations
{
    public class SpotContracts
    {

    }

    public record BatchCreateSpotRequest(IList<BatchCreateSpotRequest.Spot> Spots)
    {
        public record Spot(long ZoneId, string Name, string? Note);
    }

    public record BatchCreateSpotResponse(IList<long> IdList);

    public record CreateSpotRequest(long ZoneId, string Name, string? Note);

    public record CreateSpotResponse(long Id);

    public record UpdateSpotRequest(string Name, string? Note);

    public record GetSpotResponse(long Id, long ZoneId, string Name, string? Note);

    public record GetSpotsResponse(IList<Spot> Spots)
    {
        public record Spot(long Id, long ZoneId, string Name, string? Note);
    }
}
