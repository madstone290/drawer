using Drawer.Application.Config;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Application.Services.Inventory.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.Queries
{
    public record GetLocationByIdQuery(long Id) : IQuery<LocationQueryModel?>;

    public class GetLocationQueryHandler : IQueryHandler<GetLocationByIdQuery, LocationQueryModel?>
    {
        private readonly ILocationRepository _locationRepository;

        public GetLocationQueryHandler(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<LocationQueryModel?> Handle(GetLocationByIdQuery query, CancellationToken cancellationToken)
        {
            var location = await _locationRepository.QueryById(query.Id);

            return location;
        }
    }
}
