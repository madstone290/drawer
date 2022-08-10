using Drawer.Application.Config;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Application.Services.Inventory.Repos;

namespace Drawer.Application.Services.Inventory.Queries
{
    public record GetReceiptByIdQuery(long Id): IQuery<ReceiptQueryModel?>;

    public class GetReceiptByIdQueryHandler : IQueryHandler<GetReceiptByIdQuery, ReceiptQueryModel?>
    {
        private readonly IReceiptRepository _receiptRepository;

        public GetReceiptByIdQueryHandler(IReceiptRepository receiptRepository)
        {
            _receiptRepository = receiptRepository;
        }

        public async Task<ReceiptQueryModel?> Handle(GetReceiptByIdQuery query, CancellationToken cancellationToken)
        {
            var receipt = await _receiptRepository.GetById(query.Id);

            return receipt;
        }
    }
}
