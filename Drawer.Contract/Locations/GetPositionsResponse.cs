using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Drawer.Contract.Locations.GetPositionsResponse;

namespace Drawer.Contract.Locations
{
    public record GetPositionsResponse(IList<Position> Positions)
    {
        public record Position(long Id, string Name);
    }
}
