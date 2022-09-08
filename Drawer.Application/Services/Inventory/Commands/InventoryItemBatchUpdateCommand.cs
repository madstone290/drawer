using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.Repos;
using Drawer.Domain.Models.Inventory;
using MediatR;

namespace Drawer.Application.Services.Inventory.Commands
{
    public record InventoryItemBatchUpdateCommand(List<InventoryItemCommandModel> Items) : ICommand;

    public class InventoryItemBatchUpdateCommandHandler : ICommandHandler<InventoryItemBatchUpdateCommand>
    {
        private readonly IInventoryItemRepository _inventoryDetailRepository;
        private readonly IItemRepository _itemRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InventoryItemBatchUpdateCommandHandler(IInventoryItemRepository inventoryDetailRepository,
            IItemRepository itemRepository, ILocationRepository locationRepository, IUnitOfWork unitOfWork)
        {
            _inventoryDetailRepository = inventoryDetailRepository;
            _itemRepository = itemRepository;
            _locationRepository = locationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(InventoryItemBatchUpdateCommand command, CancellationToken cancellationToken)
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
                    
                    await _unitOfWork.CommitAsync();
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
