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
    /// 자리를 조회한다.
    /// </summary>
    public record GetSpotQuery(long Id) : IQuery<GetSpotResult?>;

    public record GetSpotResult(long Id, string Name, string? Note);

    public class GetPositionQueryHandler : IQueryHandler<GetSpotQuery, GetSpotResult?>
    {
        private readonly ISpotRepository _spotRepository;

        public GetPositionQueryHandler(ISpotRepository spotRepository)
        {
            _spotRepository = spotRepository;
        }

        public async Task<GetSpotResult?> Handle(GetSpotQuery query, CancellationToken cancellationToken)
        {
            var spot = await _spotRepository.FindByIdAsync(query.Id);

            return spot == null 
                ? null 
                : new GetSpotResult(spot.Id, spot.Name, spot.Note);
        }
    }
}
