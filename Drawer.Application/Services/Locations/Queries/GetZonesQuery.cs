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
    /// 구역 목록을 조회한다.
    /// </summary>
    public record GetZonesQuery : IQuery<GetZonesResult>;

    public record GetZonesResult(IList<GetZonesResult.Zone> Zones)
    {
        public record Zone(long Id, string Name, long? ZoneTypeId);
    }

    public class GetZonesQueryHandler : IQueryHandler<GetZonesQuery, GetZonesResult>
    {
        private readonly IZoneRepository _zoneRepository;

        public GetZonesQueryHandler(IZoneRepository ZoneRepository)
        {
            _zoneRepository = ZoneRepository;
        }

        public async Task<GetZonesResult> Handle(GetZonesQuery query, CancellationToken cancellationToken)
        {
            var zones = await _zoneRepository.FindAll();

            return new GetZonesResult(
                zones.Select(x => 
                    new GetZonesResult.Zone(x.Id, x.Name, x.ZoneTypeId)
                ).ToList());
        }
    }
}
