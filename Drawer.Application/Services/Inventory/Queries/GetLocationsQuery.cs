using Drawer.Application.Config;
using Drawer.Application.Services.Inventory.Repos;
using Drawer.Application.Services.Locations.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.Queries
{
    public record GetLocationsQuery : IQuery<GetLocationsResult>;

    public record GetLocationsResult(IList<GetLocationsResult.Location> Locations)
    {
        public record Location(long Id, long? UpperLocationId, string Name, string? Note, int HierarchyLevel, bool IsGroup);
    }

    public class GetLocationsQueryHandler : IQueryHandler<GetLocationsQuery, GetLocationsResult>
    {
        private readonly ILocationRepository _locationRepository;

        public GetLocationsQueryHandler(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<GetLocationsResult> Handle(GetLocationsQuery query, CancellationToken cancellationToken)
        {
            var locations = await _locationRepository.FindAll();

            return new GetLocationsResult(
                locations.Select(x =>
                    new GetLocationsResult.Location(x.Id, x.ParentGroupId, x.Name, x.Note, x.HierarchyLevel, x.IsGroup)
                ).ToList());
        }
    }
}
