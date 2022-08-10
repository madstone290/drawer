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
    public record GetItemsQuery : IQuery<List<ItemQueryModel>>;

    public class GetItemsQueryHandler : IQueryHandler<GetItemsQuery, List<ItemQueryModel>>
    {
        private readonly IItemRepository _positionRepository;

        public GetItemsQueryHandler(IItemRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public async Task<List<ItemQueryModel>> Handle(GetItemsQuery query, CancellationToken cancellationToken)
        {
            var items = await _positionRepository.QueryAll();

            return items;
        }
    }
}
