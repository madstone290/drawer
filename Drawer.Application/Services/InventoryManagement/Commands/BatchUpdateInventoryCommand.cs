using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.InventoryManagement.Repos;
using Drawer.Domain.Models.InventoryManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.InventoryManagement.Commands
{
    public record BatchUpdateInventoryCommand(IList<BatchUpdateInventoryCommand.InventoryChange> Changes) 
        : ICommand<BatchUpdateInventoryResult>
    {
        /// <summary>
        /// 재고상세 변화량
        /// </summary>
        /// <param name="ItemId">아이템</param>
        /// <param name="LocationId">위치</param>
        /// <param name="QuantityChange">재고 변화량(실수)</param>
        public record InventoryChange(long ItemId, long LocationId, decimal QuantityChange);
    }

    public record BatchUpdateInventoryResult;

    public class BatchUpdateInventoryCommandHandler : ICommandHandler<BatchUpdateInventoryCommand, BatchUpdateInventoryResult>
    {
        private readonly IInventoryDetailRepository _inventoryDetailRepository;
        private readonly IItemRepository _itemRepository;
        private readonly ILocationRepository _locationRepository;

        public BatchUpdateInventoryCommandHandler(IInventoryDetailRepository inventoryDetailRepository, 
            IItemRepository itemRepository, ILocationRepository locationRepository)
        {
            _inventoryDetailRepository = inventoryDetailRepository;
            _itemRepository = itemRepository;
            _locationRepository = locationRepository;
        }

        public async Task<BatchUpdateInventoryResult> Handle(BatchUpdateInventoryCommand command, CancellationToken cancellationToken)
        {
            foreach(var change in command.Changes)
            {
                var inventoryDetail = await _inventoryDetailRepository.FindByItemIdAndLocationIdAsync(change.ItemId, change.LocationId);
                if (inventoryDetail == null)
                {
                    var item = await _itemRepository.FindByIdAsync(change.ItemId)
                        ?? throw new EntityNotFoundException<Item>(change.ItemId);
                    var location = await _locationRepository.FindByIdAsync(change.LocationId)
                        ?? throw new EntityNotFoundException<Location>(change.LocationId);

                    inventoryDetail = new InventoryDetail(item, location, change.QuantityChange);
                    await _inventoryDetailRepository.AddAsync(inventoryDetail);
                    await _inventoryDetailRepository.SaveChangesAsync();
                }
                else
                {
                    inventoryDetail.Change(change.QuantityChange);
                }
            }

            return new BatchUpdateInventoryResult();
        }
    }
}
