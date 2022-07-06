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
    /// 장소 목록을 조회한다.
    /// </summary>
    public record GetWorkPlacesQuery : IQuery<GetWorkPlacesResult>;

    public record GetWorkPlacesResult(IList<GetWorkPlacesResult.WorkPlace> WorkPlaces)
    {
        public record WorkPlace(long Id, string Name, string? Note);
    }

    public class GetWorkPlacesQueryHandler : IQueryHandler<GetWorkPlacesQuery, GetWorkPlacesResult>
    {
        private readonly IWorkPlaceRepository _workPlaceRepository;

        public GetWorkPlacesQueryHandler(IWorkPlaceRepository WorkPlaceRepository)
        {
            _workPlaceRepository = WorkPlaceRepository;
        }

        public async Task<GetWorkPlacesResult> Handle(GetWorkPlacesQuery query, CancellationToken cancellationToken)
        {
            var workPlaces = await _workPlaceRepository.FindAll();

            return new GetWorkPlacesResult(
                workPlaces.Select(x => 
                    new GetWorkPlacesResult.WorkPlace(x.Id, x.Name, x.Note)
                ).ToList());
        }
    }
}
