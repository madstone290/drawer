using Drawer.Application.Config;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Application.Services.Inventory.Repos;

namespace Drawer.Application.Services.Inventory.Queries
{
    public record GetItemByIdQuery(long Id) : IQuery<ItemQueryModel?>;

    public class GetItemQueryHandler : IQueryHandler<GetItemByIdQuery, ItemQueryModel?>
    {
        private readonly IItemRepository _itemRepository;

        public GetItemQueryHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<ItemQueryModel?> Handle(GetItemByIdQuery query, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.QueryById(query.Id);

            return item;
        }
    }
}
