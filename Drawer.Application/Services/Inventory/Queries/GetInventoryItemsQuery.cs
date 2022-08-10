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
    public record GetInventoryItemsQuery(long? ItemId, long? LocationId) : IQuery<List<InventoryItemQueryModel>>;

    public class GetInventoryItemsQueryHandler : IQueryHandler<GetInventoryItemsQuery, List<InventoryItemQueryModel>>
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;

        public GetInventoryItemsQueryHandler(IInventoryItemRepository inventoryItemRepository)
        {
            _inventoryItemRepository = inventoryItemRepository;
        }

        public async Task<List<InventoryItemQueryModel>> Handle(GetInventoryItemsQuery request, CancellationToken cancellationToken)
        {
            List<InventoryItemQueryModel> inventoryItems;
            if (request.ItemId.HasValue && request.LocationId.HasValue)
            {
                var inventoryItem = await _inventoryItemRepository
                    .QueryByItemIdAndLocationId(request.ItemId.Value, request.LocationId.Value);
                inventoryItems = inventoryItem == null
                    ? new List<InventoryItemQueryModel>()
                    : new List<InventoryItemQueryModel> { inventoryItem };
            }
            else if (request.ItemId.HasValue)
            {
                inventoryItems = await _inventoryItemRepository.QueryByItemId(request.ItemId.Value);
            }
            else if (request.LocationId.HasValue)
            {
                inventoryItems = await _inventoryItemRepository.QueryByLocationId(request.LocationId.Value);
            }
            else
            {
                inventoryItems = await _inventoryItemRepository.QueryAll();
            }

            return inventoryItems;
        }
    }
}
