using Drawer.Application.Config;
using Drawer.Application.Services.Locations.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Locations.Queries
{
    /// <summary>
    /// 구역을 조회한다.
    /// </summary>
    public record GetZoneQuery(long Id) : IQuery<GetZoneResult?>;

    public record GetZoneResult(long Id, long WorkPlaceId, string Name, string? Note);

    public class GetZoneQueryHandler : IQueryHandler<GetZoneQuery, GetZoneResult?>
    {
        private readonly IZoneRepository _zoneRepository;

        public GetZoneQueryHandler(IZoneRepository zoneRepository)
        {
            _zoneRepository = zoneRepository;
        }

        public async Task<GetZoneResult?> Handle(GetZoneQuery query, CancellationToken cancellationToken)
        {
            var zone = await _zoneRepository.FindByIdAsync(query.Id);

            return zone == null 
                ? null 
                : new GetZoneResult(zone.Id, zone.WorkPlaceId, zone.Name, zone.Note);
        }
    }
}
