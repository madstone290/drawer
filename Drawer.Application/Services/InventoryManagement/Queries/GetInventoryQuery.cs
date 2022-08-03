using Drawer.Application.Config;
using Drawer.Application.Services.InventoryManagement.Repos;
using Drawer.Domain.Models.InventoryManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.InventoryManagement.Queries
{
    public record GetInventoryQuery(long? ItemId, long? LocationId) : IQuery<GetInventoryResult>;

    public record GetInventoryResult(IList<GetInventoryResult.InventoryDetail> InventoryDetails)
    {
        public record InventoryDetail(long ItemId, long LocationId, decimal Quantity);
    }

    public class GetInventoryQueryHandler : IQueryHandler<GetInventoryQuery, GetInventoryResult>
    {
        private readonly IInventoryDetailRepository _inventoryDetailRepository;

        public GetInventoryQueryHandler(IInventoryDetailRepository inventoryDetailRepository)
        {
            _inventoryDetailRepository = inventoryDetailRepository;
        }

        public async Task<GetInventoryResult> Handle(GetInventoryQuery request, CancellationToken cancellationToken)
        {
            IList<InventoryDetail> inventoryDetails;
            if (request.ItemId.HasValue && request.LocationId.HasValue)
            {
                var inventoryDetail = await _inventoryDetailRepository.FindByItemIdAndLocationIdAsync(
                    request.ItemId.Value, request.LocationId.Value);
                inventoryDetails = inventoryDetail == null
                    ? new List<InventoryDetail>()
                    : new List<InventoryDetail> { inventoryDetail };
            }
            else if (request.ItemId.HasValue)
            {
                inventoryDetails = await _inventoryDetailRepository.FindByItemIdAsync(request.ItemId.Value);
            }
            else if (request.LocationId.HasValue)
            {
                inventoryDetails = await _inventoryDetailRepository.FindByLocationIdAsync(request.LocationId.Value);
            }
            else
            {
                inventoryDetails = await _inventoryDetailRepository.FindAll();
            }

            return new GetInventoryResult(inventoryDetails.Select(x =>
                new GetInventoryResult.InventoryDetail(x.ItemId, x.LocationId, x.Quantity)).ToList());
        }
    }
}
