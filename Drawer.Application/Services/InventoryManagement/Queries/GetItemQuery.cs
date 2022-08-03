using Drawer.Application.Config;
using Drawer.Application.Services.InventoryManagement.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.InventoryManagement.Queries
{
    public record GetItemQuery(long Id) : IQuery<GetItemResult?>;

    public record GetItemResult(long Id, string Name, string? Code, string? Number,
        string? Sku, string? QuantityUnit);

    public class GetItemQueryHandler : IQueryHandler<GetItemQuery, GetItemResult?>
    {
        private readonly IItemRepository _itemRepository;

        public GetItemQueryHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<GetItemResult?> Handle(GetItemQuery query, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.FindByIdAsync(query.Id);

            return item == null
                ? null
                : new GetItemResult(item.Id, item.Name, item.Code, item.Number, item.Sku, item.QuantityUnit);
        }
    }
}
