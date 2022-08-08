﻿using Drawer.Application.Config;
using Drawer.Application.Services.Inventory.Repos;
using Drawer.Domain.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.Queries
{
    public record GetReceiptsQuery(DateTime From, DateTime To) : IQuery<GetReceiptsResult?>;

    public record GetReceiptsResult(List<GetReceiptsResult.Receipt> Receipts) 
    {
        public record Receipt(long Id, long ItemId, long LocationId, decimal Quantity, DateTime ReceiptTime, string? Seller);
    }

    public class GetReceiptsQueryHandler : IQueryHandler<GetReceiptsQuery, GetReceiptsResult?>
    {
        private readonly IReceiptRepository _receiptRepository;

        public GetReceiptsQueryHandler(IReceiptRepository receiptRepository)
        {
            _receiptRepository = receiptRepository;
        }

        public async Task<GetReceiptsResult?> Handle(GetReceiptsQuery query, CancellationToken cancellationToken)
        {
            var receipts = await _receiptRepository.FindByReceiptDateBetween(query.From, query.To);

            return new GetReceiptsResult(receipts.Select(x => 
                new GetReceiptsResult.Receipt(x.Id, x.ItemId, x.LocationId, x.Quantity, x.ReceiptTimeLocal, x.Seller)).ToList());
        }
    }
}
