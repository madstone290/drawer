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
    public record GetLocationsQuery : IQuery<List<LocationQueryModel>>;

    public class GetLocationsQueryHandler : IQueryHandler<GetLocationsQuery, List<LocationQueryModel>>
    {
        private readonly ILocationRepository _locationRepository;

        public GetLocationsQueryHandler(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<List<LocationQueryModel>> Handle(GetLocationsQuery query, CancellationToken cancellationToken)
        {
            var locations = await _locationRepository.QueryAll();

            return locations;
        }
    }
}
