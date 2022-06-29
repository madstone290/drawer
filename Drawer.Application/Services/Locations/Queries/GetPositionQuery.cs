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
    /// 하나의 위치를 조회한다.
    /// </summary>
    public record GetPositionQuery(long Id) : IQuery<GetPositionResult?>;

    public record GetPositionResult(long Id, string Name);

    public class GetPositionQueryHandler : IQueryHandler<GetPositionQuery, GetPositionResult?>
    {
        private readonly IPositionRepository _positionRepository;

        public GetPositionQueryHandler(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public async Task<GetPositionResult?> Handle(GetPositionQuery query, CancellationToken cancellationToken)
        {
            var position = await _positionRepository.FindByIdAsync(query.Id);

            return position == null 
                ? null 
                : new GetPositionResult(position.Id, position.Name);
        }
    }
}
