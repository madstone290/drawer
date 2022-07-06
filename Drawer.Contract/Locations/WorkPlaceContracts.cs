using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Drawer.Contract.Locations.GetWorkPlacesResponse;

namespace Drawer.Contract.Locations
{
    public class WorkPlaceContracts
    {
    }

    public record CreateWorkPlaceRequest(string Name, string? Note);

    public record CreateWorkPlaceResponse(long Id);

    public record UpdateWorkPlaceRequest(string Name, string? Note);

    public record GetWorkPlaceResponse(long Id, string Name, string? Note);

    public record GetWorkPlacesResponse(IList<WorkPlace> WorkPlaces)
    {
        public record WorkPlace(long Id, string Name, string? Note);
    }
}
