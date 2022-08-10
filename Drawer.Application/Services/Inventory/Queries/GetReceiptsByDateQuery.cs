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
    public class GetReceiptsByDateQuery : IQuery<List<ReceiptQueryModel>>
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

    public class GetReceiptsByDateHandler : IQueryHandler<GetReceiptsByDateQuery, List<ReceiptQueryModel>>
    {
        private readonly IReceiptRepository _receiptRepository;

        public GetReceiptsByDateHandler(IReceiptRepository receiptRepository)
        {
            _receiptRepository = receiptRepository;
        }

        public async Task<List<ReceiptQueryModel>> Handle(GetReceiptsByDateQuery query, CancellationToken cancellationToken)
        {
            var receipts = await _receiptRepository.GetByReceiptDateBetween(query.From, query.To);

            return receipts;
        }
    }
}
