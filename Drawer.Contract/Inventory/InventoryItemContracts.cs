using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Contract.Inventory
{
    public class InventoryItemContracts
    {
    }

    public record BatchUpdateInventoryItemRequest(List<BatchUpdateInventoryItemRequest.InventoryItem> InventoryItemChanges)
    {
        public record InventoryItem(long ItemId, long LocationId, decimal QuantityChange);
    }

    public record UpdateInventoryRequest(long ItemId, long LocationId, decimal QuantityChange);

    public record GetInventoryItemsResponse(List<GetInventoryItemsResponse.InventoryItem> InventoryItems)
    {
        public record InventoryItem(long ItemId, long LocationId, decimal Quantity);
    }

}
