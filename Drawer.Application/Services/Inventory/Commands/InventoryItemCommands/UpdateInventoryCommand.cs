using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.Repos;
using Drawer.Domain.Models.Inventory;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.Commands.InventoryItemCommands
{
    public record UpdateInventoryCommand(InventoryItemUpdateCommandModel Item) : ICommand;

    public class UpdateInventoryCommandHandler : ICommandHandler<UpdateInventoryCommand>
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

        public async Task<Unit> Handle(UpdateInventoryCommand command, CancellationToken cancellationToken)
        {
            var itemDto = command.Item;

            var inventoryItem = await _inventoryDetailRepository.FindByItemIdAndLocationIdAsync(itemDto.ItemId, itemDto.LocationId);
            if (inventoryItem == null)
            {
                if (!await _itemRepository.ExistByIdAsync(itemDto.ItemId))
                    throw new EntityNotFoundException<Item>(itemDto.ItemId);
                if (!await _locationRepository.ExistByIdAsync(itemDto.LocationId))
                    throw new EntityNotFoundException<Location>(itemDto.LocationId);

                inventoryItem = new InventoryItem(itemDto.ItemId, itemDto.LocationId, itemDto.QuantityChange);
                await _inventoryDetailRepository.AddAsync(inventoryItem);
                await _inventoryDetailRepository.SaveChangesAsync();
            }
            else
            {
                inventoryItem.Add(itemDto.QuantityChange);
            }

            return Unit.Value;
        }
    }
}
