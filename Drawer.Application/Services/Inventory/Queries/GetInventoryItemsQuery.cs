using Drawer.Application.Config;
using Drawer.Application.Services.Inventory.Repos;
using Drawer.Domain.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.Queries
{
    public record GetInventoryItemsQuery(long? ItemId, long? LocationId) : IQuery<GetInventoryItemsResult>;

    public record GetInventoryItemsResult(IList<GetInventoryItemsResult.InventoryItem> InventoryItems)
    {
        public record InventoryItem(long ItemId, long LocationId, decimal Quantity);
    }

    public class GetInventoryItemsQueryHandler : IQueryHandler<GetInventoryItemsQuery, GetInventoryItemsResult>
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;

        public GetInventoryItemsQueryHandler(IInventoryItemRepository inventoryItemRepository)
        {
            _inventoryItemRepository = inventoryItemRepository;
        }

        public async Task<GetInventoryItemsResult> Handle(GetInventoryItemsQuery request, CancellationToken cancellationToken)
        {
            IList<InventoryItem> inventoryItems;
            if (request.ItemId.HasValue && request.LocationId.HasValue)
            {
                var inventoryItem = await _inventoryItemRepository
                    .FindByItemIdAndLocationIdAsync(request.ItemId.Value, request.LocationId.Value);
                inventoryItems = inventoryItem == null
                    ? new List<InventoryItem>()
                    : new List<InventoryItem> { inventoryItem };
            }
            else if (request.ItemId.HasValue)
            {
                inventoryItems = await _inventoryItemRepository.FindByItemIdAsync(request.ItemId.Value);
            }
            else if (request.LocationId.HasValue)
            {
                inventoryItems = await _inventoryItemRepository.FindByLocationIdAsync(request.LocationId.Value);
            }
            else
            {
                inventoryItems = await _inventoryItemRepository.FindAll();
            }

            return new GetInventoryItemsResult(inventoryItems.Select(x =>
                new GetInventoryItemsResult.InventoryItem(x.ItemId, x.LocationId, x.Quantity)).ToList());
        }
    }
}
