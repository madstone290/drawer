using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.Repos;
using Drawer.Domain.Models.Inventory;
using MediatR;

namespace Drawer.Application.Services.Inventory.Commands.InventoryItemCommands
{
    public record BatchUpdateInventoryCommand(List<InventoryItemUpdateCommandModel> Items) : ICommand;

    public class BatchUpdateInventoryCommandHandler : ICommandHandler<BatchUpdateInventoryCommand>
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

        public async Task<Unit> Handle(BatchUpdateInventoryCommand command, CancellationToken cancellationToken)
        {
            foreach (var itemDto in command.Items)
            {
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
            }

            return Unit.Value;
        }
    }
}
