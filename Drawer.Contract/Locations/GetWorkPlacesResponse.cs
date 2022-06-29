using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Drawer.Contract.Locations.GetWorkPlacesResponse;

namespace Drawer.Contract.Locations
{
    public record GetWorkPlacesResponse(IList<WorkPlace> WorkPlaces)
    {
        public record WorkPlace(long Id, string Name, string? Description);
    }
}
