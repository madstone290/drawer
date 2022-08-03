using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Contract.BasicInfo
{
    public class LocationContracts
    {
    }

    public record BatchCreateLocationRequest(IList<BatchCreateLocationRequest.Location> Locations)
    {
        public record Location(long? UpperLocationId, string Name, string? Note);
    }

    public record BatchCreateLocationResponse(IList<long> IdList);

    public record CreateLocationRequest(long? UpperLocationId, string Name, string? Note);

    public record CreateLocationResponse(long Id);

    public record UpdateLocationRequest(string Name, string? Note);

    public record GetLocationResponse(long Id, long? UpperLocationId, string Name, string? Note);

    public record GetLocationsResponse(IList<GetLocationsResponse.Location> Locations)
    {
        public record Location(long Id, long? UpperLocationId, string Name, string? Note);
    }
}
