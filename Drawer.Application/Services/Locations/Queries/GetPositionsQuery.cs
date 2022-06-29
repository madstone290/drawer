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
    /// 위치 목록을 조회한다.
    /// </summary>
    public record GetPositionsQuery : IQuery<GetPositionsResult>;

    public record GetPositionsResult(IList<GetPositionsResult.Position> Positions)
    {
        public record Position(long Id, string Name);
    }

    public class GetPositionsQueryHandler : IQueryHandler<GetPositionsQuery, GetPositionsResult>
    {
        private readonly IPositionRepository _positionRepository;

        public GetPositionsQueryHandler(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public async Task<GetPositionsResult> Handle(GetPositionsQuery query, CancellationToken cancellationToken)
        {
            var positions = await _positionRepository.FindAll();

            return new GetPositionsResult(
                positions.Select(x => 
                    new GetPositionsResult.Position(x.Id, x.Name)
                ).ToList());
        }
    }
}
