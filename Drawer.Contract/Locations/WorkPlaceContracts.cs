using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Drawer.Contract.Locations.GetWorkplacesResponse;

namespace Drawer.Contract.Locations
{
    public class WorkplaceContracts
    {
    }

    public record BatchCreateWorkplaceRequest(IList<BatchCreateWorkplaceRequest.Workplace> Workplaces)
    {
        public record Workplace(string Name, string? Note);
    }

    public record BatchCreateWorkplaceResponse(IList<long> IdList);

    public record CreateWorkplaceRequest(string Name, string? Note);

    public record CreateWorkplaceResponse(long Id);

    public record UpdateWorkplaceRequest(string Name, string? Note);

    public record GetWorkplaceResponse(long Id, string Name, string? Note);

    public record GetWorkplacesResponse(IList<Workplace> Workplaces)
    {
        public record Workplace(long Id, string Name, string? Note);
    }
}
