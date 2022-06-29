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
    /// 하나의 구역유형을 조회한다.
    /// </summary>
    public record GetZoneTypeQuery(long Id) : IQuery<GetZoneTypeResult?>;

    public record GetZoneTypeResult(long Id, string Name);

    public class GetZoneTypeQueryHandler : IQueryHandler<GetZoneTypeQuery, GetZoneTypeResult?>
    {
        private readonly IZoneTypeRepository _zoneTypeRepository;

        public GetZoneTypeQueryHandler(IZoneTypeRepository zoneTypeRepository)
        {
            _zoneTypeRepository = zoneTypeRepository;
        }

        public async Task<GetZoneTypeResult?> Handle(GetZoneTypeQuery query, CancellationToken cancellationToken)
        {
            var position = await _zoneTypeRepository.FindByIdAsync(query.Id);

            return position == null
                ? null
                : new GetZoneTypeResult(position.Id, position.Name);
        }
    }
}
