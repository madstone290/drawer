using Drawer.Application.Config;
using Drawer.Application.Services.Items.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Items.Queries
{
    public record GetItemsQuery : IQuery<GetItemsResult>;

    public record GetItemsResult(IList<GetItemsResult.Item> Items)
    {
        public record Item(long Id, string Name, string? Code, string? Number,
            string? Sku, string? MeasurementUnit);
    }

    public class GetItemsQueryHandler : IQueryHandler<GetItemsQuery, GetItemsResult>
    {
        private readonly IItemRepository _positionRepository;

        public GetItemsQueryHandler(IItemRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public async Task<GetItemsResult> Handle(GetItemsQuery query, CancellationToken cancellationToken)
        {
            var items = await _positionRepository.FindAll();

            return new GetItemsResult(
                items.Select(item => 
                    new GetItemsResult.Item(item.Id, item.Name, item.Code, item.Number, item.Sku, item.MeasurementUnit)
                ).ToList());
        }
    }
}
