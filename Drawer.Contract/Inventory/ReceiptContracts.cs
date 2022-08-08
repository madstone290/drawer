using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Contract.Inventory
{
    public class ReceiptContracts
    {
    }

    public record CreateReceiptRequest(long ItemId, long LocationId, decimal Quantity, DateTime ReceiptTime, string? Seller);

    public record CreateReceiptResponse(long Id);

    public record UpdateReceiptRequest(long ItemId, long LocationId, decimal Quantity, DateTime ReceiptTime, string? Seller);

    public record GetReceiptResponse(long Id, long ItemId, long LocationId, decimal Quantity, DateTime ReceiptTime, string? Seller);

    public record GetReceiptsResponse(List<GetReceiptsResponse.Receipt> Receipts)
    {
        public record Receipt(long Id, long ItemId, long LocationId, decimal Quantity, DateTime ReceiptTime, string? Seller);
    }
}
