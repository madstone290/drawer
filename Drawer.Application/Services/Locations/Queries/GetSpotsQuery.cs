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
    /// 자리 목록을 조회한다.
    /// </summary>
    public record GetSpotsQuery : IQuery<GetSpotsResult>;

    public record GetSpotsResult(IList<GetSpotsResult.Spot> Spots)
    {
        public record Spot(long Id, string Name, string? Note);
    }

    public class GetPositionsQueryHandler : IQueryHandler<GetSpotsQuery, GetSpotsResult>
    {
        private readonly ISpotRepository _positionRepository;

        public GetPositionsQueryHandler(ISpotRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public async Task<GetSpotsResult> Handle(GetSpotsQuery query, CancellationToken cancellationToken)
        {
            var positions = await _positionRepository.FindAll();

            return new GetSpotsResult(
                positions.Select(x => 
                    new GetSpotsResult.Spot(x.Id, x.Name, x.Note)
                ).ToList());
        }
    }
}
