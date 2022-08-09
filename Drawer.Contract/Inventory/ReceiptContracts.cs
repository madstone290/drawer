using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Contract.Inventory
{
    public class ReceiptContracts
    {
        public record Receipt(long Id, string TransactionNumber, DateTime ReceiptDateTime, long ItemId, long LocationId, decimal Quantity, string? Seller);
    }

    public record CreateReceiptRequest(DateTime ReceiptDateTime, long ItemId, long LocationId, decimal Quantity, string? Seller);

    public record CreateReceiptResponse(long Id, string TransactionNumber);

    public record UpdateReceiptRequest(DateTime ReceiptDateTime, long ItemId, long LocationId, decimal Quantity, string? Seller);

    public record GetReceiptResponse(ReceiptContracts.Receipt Receipt);

    public record GetReceiptsResponse(List<ReceiptContracts.Receipt> Receipts);
}
