using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.Repos;
using Drawer.Domain.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.Commands.ReceiptCommands
{
    /// <summary>
    /// 입고를 생성하고 입고ID를 반환한다.
    /// </summary>
    public record CreateReceiptCommand(ReceiptAddUpdateCommandModel Receipt) : ICommand<long>;

    public class CreateReceiptCommandHandler : ICommandHandler<CreateReceiptCommand, long>
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

        public async Task<long> Handle(CreateReceiptCommand command, CancellationToken cancellationToken)
        {
            var receiptDto = command.Receipt;

            // 입고내역 생성 후 재고 증가

            // 입고내역 생성
            if (!await _itemRepository.ExistByIdAsync(receiptDto.ItemId))
                throw new EntityNotFoundException<Item>(receiptDto.ItemId);
            if (!await _locationRepository.ExistByIdAsync(receiptDto.LocationId))
                throw new EntityNotFoundException<Location>(receiptDto.LocationId);

            var transactionNumber = Guid.NewGuid().ToString();
            var receipt = new Receipt(transactionNumber, receiptDto.ItemId, receiptDto.LocationId, receiptDto.Quantity);
            receipt.SetReceiptDateTime(receiptDto.ReceiptDateTime);
            receipt.SetSeller(receiptDto.Seller);

            await _inventoryUnitOfWork.ReceiptRepository.AddAsync(receipt);

            // 재고 증가
            var inventoryItem = await _inventoryUnitOfWork.InventoryItemRepository
               .FindByItemIdAndLocationIdAsync(receiptDto.ItemId, receiptDto.LocationId);
            if (inventoryItem == null)
            {
                inventoryItem = new InventoryItem(receiptDto.ItemId, receiptDto.LocationId, receiptDto.Quantity);
                await _inventoryUnitOfWork.InventoryItemRepository.AddAsync(inventoryItem);
            }
            else
            {
                inventoryItem.Increase(receiptDto.Quantity);
            }

            await _inventoryUnitOfWork.SaveChangesAsync();
            return receipt.Id;
        }
    }
}

