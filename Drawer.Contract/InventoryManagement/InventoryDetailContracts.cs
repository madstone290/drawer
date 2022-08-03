using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Contract.InventoryManagement
{
    public class InventoryContracts
    {
    }

    public record BatchUpdateInventoryRequest(List<BatchUpdateInventoryRequest.InventoryChange> Changes)
    {
        public record InventoryChange(long ItemId, long LocationId, decimal QuantityChange);
    }

    public record UpdateInventoryRequest(long ItemId, long LocationId, decimal QuantityChange);

    public record GetInventoryResponse(List<GetInventoryResponse.InventoryDetail> InventoryDetails)
    {
        public record InventoryDetail(long ItemId, long LocationId, decimal Quantity);
    }

}
