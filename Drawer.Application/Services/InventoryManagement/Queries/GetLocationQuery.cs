using Drawer.Application.Config;
using Drawer.Application.Services.InventoryManagement.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.InventoryManagement.Queries
{
    public record GetLocationQuery(long Id) : IQuery<GetLocationResult?>;

    public record GetLocationResult(long Id, long? UpperLocationId, string Name, string? Note);

    public class GetLocationQueryHandler : IQueryHandler<GetLocationQuery, GetLocationResult?>
    {
        private readonly ILocationRepository _locationRepository;

        public GetLocationQueryHandler(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<GetLocationResult?> Handle(GetLocationQuery query, CancellationToken cancellationToken)
        {
            var location = await _locationRepository.FindByIdAsync(query.Id);

            return location == null
                ? null
                : new GetLocationResult(location.Id, location.UpperLocationId, location.Name, location.Note);
        }
    }
}
