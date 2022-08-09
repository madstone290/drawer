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
    public record GetReceiptQuery(long Id) : IQuery<GetReceiptResult?>;

    public record GetReceiptResult(Receipt? Receipt);

    public class GetReceiptQueryHandler : IQueryHandler<GetReceiptQuery, GetReceiptResult?>
    {
        private readonly IReceiptRepository _receiptRepository;

        public GetReceiptQueryHandler(IReceiptRepository receiptRepository)
        {
            _receiptRepository = receiptRepository;
        }

        public async Task<GetReceiptResult?> Handle(GetReceiptQuery query, CancellationToken cancellationToken)
        {
            var receipt = await _receiptRepository.FindByIdAsync(query.Id);

            return new GetReceiptResult(receipt);
        }
    }
}
