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
    /// 구역유형 목록을 조회한다.
    /// </summary>
    public record GetZoneTypesQuery : IQuery<GetZoneTypesResult>;

    public record GetZoneTypesResult(IList<GetZoneTypesResult.ZoneType> ZoneTypes)
    {
        public record ZoneType(long Id, string Name);
    }

    public class GetZoneTypesQueryHandler : IQueryHandler<GetZoneTypesQuery, GetZoneTypesResult>
    {
        private readonly IZoneTypeRepository _zoneTypeRepository;

        public GetZoneTypesQueryHandler(IZoneTypeRepository zoneTypeRepository)
        {
            _zoneTypeRepository = zoneTypeRepository;
        }

        public async Task<GetZoneTypesResult> Handle(GetZoneTypesQuery query, CancellationToken cancellationToken)
        {
            var zoneTypes = await _zoneTypeRepository.FindAll();

            return new GetZoneTypesResult(
                zoneTypes.Select(x => 
                    new GetZoneTypesResult.ZoneType(x.Id, x.Name)
                ).ToList());
        }
    }
}
