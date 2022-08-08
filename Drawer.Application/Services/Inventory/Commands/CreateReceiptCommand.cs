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
    public record CreateReceiptCommand(long ItemId, long LocationId, decimal Quantity, DateTime When, string? Seller) 
        : ICommand<CreateReceiptResult>;

    public record CreateReceiptResult(long Id);

    public class CreateReceiptCommandHandler : ICommandHandler<CreateReceiptCommand, CreateReceiptResult>
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        private readonly IItemRepository _itemRepository;
        private readonly ILocationRepository _locationRepository;

        public CreateReceiptCommandHandler(IInventoryUnitOfWork inventoryUnitOfWork,
                                           IItemRepository itemRepository,
                                           ILocationRepository locationRepository)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
            _itemRepository = itemRepository;
            _locationRepository = locationRepository;
        }

        public async Task<CreateReceiptResult> Handle(CreateReceiptCommand command, CancellationToken cancellationToken)
        {
            // 입고내역 생성 후 재고 증가

            // 입고내역 생성
            if (!await _itemRepository.ExistByIdAsync(command.ItemId))
                throw new EntityNotFoundException<Item>(command.ItemId);
            if (!await _locationRepository.ExistByIdAsync(command.LocationId))
                throw new EntityNotFoundException<Location>(command.LocationId);

            var receipt = new Receipt(command.ItemId, command.LocationId, command.Quantity);
            receipt.SetReceiptTime(command.When);
            receipt.SetSeller(command.Seller);

            await _inventoryUnitOfWork.ReceiptRepository.AddAsync(receipt);

            // 재고 증가
            var inventoryItem = await _inventoryUnitOfWork.InventoryItemRepository
               .FindByItemIdAndLocationIdAsync(command.ItemId, command.LocationId);
            if (inventoryItem == null)
            {
                inventoryItem = new InventoryItem(command.ItemId, command.LocationId, command.Quantity);
                await _inventoryUnitOfWork.InventoryItemRepository.AddAsync(inventoryItem);
            }
            else
            {
                inventoryItem.Increase(command.Quantity);
            }

            await _inventoryUnitOfWork.SaveChangesAsync();
            return new CreateReceiptResult(receipt.Id);
        }
    }
}

