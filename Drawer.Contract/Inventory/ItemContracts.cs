using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Contract.Inventory
{
    public class ItemContracts
    {
    }

    public record BatchCreateItemRequest(IList<BatchCreateItemRequest.Item> Items)
    {
        public record Item(string Name, string? Code, string? Number, string? Sku, string? QuantityUnit);
    }

    public record BatchCreateItemResponse(IList<long> IdList);

    public record CreateItemRequest(string Name, string? Code, string? Number,
       string? Sku, string? QuantityUnit);

    public record CreateItemResponse(long Id);

    public record GetItemResponse(long Id, string Name, string? Code, string? Number, string? Sku, string? QuantityUnit);

    public record GetItemsResponse(IList<GetItemsResponse.Item> Items)
    {
        public record Item(long Id, string Name, string? Code, string? Number, string? Sku, string? QuantityUnit);
    }

    public record UpdateItemRequest(string Name, string? Code, string? Number, string? Sku, string? QuantityUnit);
}
