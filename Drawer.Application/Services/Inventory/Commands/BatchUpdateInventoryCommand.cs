using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Inventory.Repos;
using Drawer.Domain.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.Commands
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
        private readonly IInventoryItemRepository _inventoryDetailRepository;
        private readonly IItemRepository _itemRepository;
        private readonly ILocationRepository _locationRepository;

        public BatchUpdateInventoryCommandHandler(IInventoryItemRepository inventoryDetailRepository, 
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
                var inventoryItem = await _inventoryDetailRepository.FindByItemIdAndLocationIdAsync(change.ItemId, change.LocationId);
                if (inventoryItem == null)
                {
                    if (!await _itemRepository.ExistByIdAsync(change.ItemId))
                        throw new EntityNotFoundException<Item>(change.ItemId);
                    if (!await _locationRepository.ExistByIdAsync(change.LocationId))
                        throw new EntityNotFoundException<Location>(change.LocationId);
                    
                    inventoryItem = new InventoryItem(change.ItemId, change.LocationId, change.QuantityChange);
                    await _inventoryDetailRepository.AddAsync(inventoryItem);
                    await _inventoryDetailRepository.SaveChangesAsync();
                }
                else
                {
                    inventoryItem.Add(change.QuantityChange);
                }
            }

            return new BatchUpdateInventoryResult();
        }
    }
}
