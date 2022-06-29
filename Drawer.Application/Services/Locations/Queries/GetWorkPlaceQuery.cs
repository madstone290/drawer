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
    /// 하나의 작업장 조회한다.
    /// </summary>
    /// <param name="Id">작업장 Id</param>
    public record GetWorkPlaceQuery(long Id) : IQuery<GetWorkPlaceResult?>;

    public record GetWorkPlaceResult(long Id, string Name, string? Description);

    public class GetWorkPlaceQueryHandler : IQueryHandler<GetWorkPlaceQuery, GetWorkPlaceResult?>
    {
        private readonly IWorkPlaceRepository _workPlaceRepository;

        public GetWorkPlaceQueryHandler(IWorkPlaceRepository workPlaceRepository)
        {
            _workPlaceRepository = workPlaceRepository;
        }

        public async Task<GetWorkPlaceResult?> Handle(GetWorkPlaceQuery query, CancellationToken cancellationToken)
        {
            var workPlace = await _workPlaceRepository.FindByIdAsync(query.Id);

            return workPlace == null 
                ? null 
                : new GetWorkPlaceResult(workPlace.Id, workPlace.Name, workPlace.Description);
        }
    }
}
