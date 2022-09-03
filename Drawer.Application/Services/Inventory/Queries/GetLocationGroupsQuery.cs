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
    public record GetLocationGroupsQuery : IQuery<List<LocationGroupQueryModel>>;

    public class GetLocationGroupsQueryHandler : IQueryHandler<GetLocationGroupsQuery, List<LocationGroupQueryModel>>
    {
        private readonly ILocationGroupRepository _groupRepository;

        public GetLocationGroupsQueryHandler(ILocationGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<List<LocationGroupQueryModel>> Handle(GetLocationGroupsQuery query, CancellationToken cancellationToken)
        {
            var groups = await _groupRepository.QueryAll();

            return groups;
        }
    }
}
