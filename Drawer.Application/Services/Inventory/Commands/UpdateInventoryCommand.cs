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
    /// <summary>
    /// 재고를 수정한다.
    /// </summary>
    /// <param name="ItemId">아이템</param>
    /// <param name="LocationId">위치</param>
    /// <param name="QuantityChange">재고 변화량(실수)</param>
    public record UpdateInventoryCommand(long ItemId, long LocationId, decimal QuantityChange) : ICommand<UpdateInventoryResult>;

    public record UpdateInventoryResult;

    public class UpdateInventoryCommandHandler : ICommandHandler<UpdateInventoryCommand, UpdateInventoryResult>
    {
        private readonly IInventoryItemRepository _inventoryDetailRepository;
        private readonly IItemRepository _itemRepository;
        private readonly ILocationRepository _locationRepository;

        public UpdateInventoryCommandHandler(IInventoryItemRepository inventoryDetailRepository,
            IItemRepository itemRepository, ILocationRepository locationRepository)
        {
            _inventoryDetailRepository = inventoryDetailRepository;
            _itemRepository = itemRepository;
            _locationRepository = locationRepository;
        }

        public async Task<UpdateInventoryResult> Handle(UpdateInventoryCommand command, CancellationToken cancellationToken)
        {
            var inventoryItem = await _inventoryDetailRepository.FindByItemIdAndLocationIdAsync(command.ItemId, command.LocationId);
            if (inventoryItem == null)
            {
                if (!await _itemRepository.ExistByIdAsync(command.ItemId))
                    throw new EntityNotFoundException<Item>(command.ItemId);
                if (!await _locationRepository.ExistByIdAsync(command.LocationId))
                    throw new EntityNotFoundException<Location>(command.LocationId);

                inventoryItem = new InventoryItem(command.ItemId, command.LocationId, command.QuantityChange);
                await _inventoryDetailRepository.AddAsync(inventoryItem);
                await _inventoryDetailRepository.SaveChangesAsync();
            }
            else
            {
                inventoryItem.Add(command.QuantityChange);
            }

            return new UpdateInventoryResult();
        }
    }
}
