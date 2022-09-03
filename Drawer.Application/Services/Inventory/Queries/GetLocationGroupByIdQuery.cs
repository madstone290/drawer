using Drawer.Application.Config;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Application.Services.Inventory.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.Queries
{
    public record GetLocationGroupByIdQuery(long Id) : IQuery<LocationGroupQueryModel?>;

    public class GetLocationGroupQueryHandler : IQueryHandler<GetLocationGroupByIdQuery, LocationGroupQueryModel?>
    {
        private readonly ILocationGroupRepository _groupRepository;

        public GetLocationGroupQueryHandler(ILocationGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<LocationGroupQueryModel?> Handle(GetLocationGroupByIdQuery query, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.QueryById(query.Id);

            return group;
        }
    }
}
