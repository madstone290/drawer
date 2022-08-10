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

namespace Drawer.Application.Services.Inventory.Commands.ReceiptCommands
{
    public record UpdateReceiptCommand(long Id, ReceiptAddUpdateCommandModel Receipt) : ICommand<Unit>;

    public class UpdateReceiptHandler : ICommandHandler<UpdateReceiptCommand, Unit>
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        private readonly IItemRepository _itemRepository;
        private readonly ILocationRepository _locationRepository;

        public UpdateReceiptHandler(IInventoryUnitOfWork inventoryUnitOfWork,
                                    IItemRepository itemRepository,
                                    ILocationRepository locationRepository)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
            _itemRepository = itemRepository;
            _locationRepository = locationRepository;
        }

        public async Task<Unit> Handle(UpdateReceiptCommand command, CancellationToken cancellationToken)
        {
            var receiptId = command.Id;
            var receiptDto = command.Receipt;

            // 1. 품목, 위치가 같은 경우 (동일한 1개의 재고를 수정한다)
            // 2. 품목, 위치가 다른 경우 (다른 2개의 재고를 수정한다)

            var receipt = await _inventoryUnitOfWork.ReceiptRepository
                .FindByIdAsync(receiptId) ?? throw new EntityNotFoundException<Receipt>(receiptId);


            if (receiptDto.ItemId == receipt.ItemId && receiptDto.LocationId == receipt.LocationId)
            {
                // 품목, 위치가 같은 경우 
                // 1. 입고내역 수정
                // 2. 기존 재고 수정

                var quantityDiff = receiptDto.Quantity - receipt.Quantity;
                var inventoryItem = await _inventoryUnitOfWork.InventoryItemRepository
                    .FindByItemIdAndLocationIdAsync(receipt.ItemId, receipt.LocationId);
                if (inventoryItem == null || inventoryItem.Quantity + quantityDiff < 0)
                    throw new AppException("재고수량이 부족하여 입고내역을 수정할 수 없습니다");

                receipt.SetQuantity(receiptDto.Quantity);
                receipt.SetReceiptDateTime(receiptDto.ReceiptDateTime);
                receipt.SetSeller(receiptDto.Seller);

                inventoryItem.Add(quantityDiff);
            }
            else
            {
                // 품목, 위치가 다른 경우 
                // 1. 입고내역 수정
                // 2. 이전 재고 감소
                // 3. 이후 재고 증가

                // 재고수량 확인
                var beforeInventoryItem = await _inventoryUnitOfWork.InventoryItemRepository
                    .FindByItemIdAndLocationIdAsync(receipt.ItemId, receipt.LocationId);
                if (beforeInventoryItem == null || beforeInventoryItem.Quantity - receipt.Quantity < 0)
                    throw new AppException("재고수량이 부족하여 입고내역을 수정할 수 없습니다");

                // 입고내역 수정
                if (!await _itemRepository.ExistByIdAsync(receiptDto.ItemId))
                    throw new EntityNotFoundException<Item>(receiptDto.ItemId);
                if (!await _locationRepository.ExistByIdAsync(receiptDto.LocationId))
                    throw new EntityNotFoundException<Location>(receiptDto.LocationId);

                var quantityBefore = receipt.Quantity;

                receipt.SetInventoryInfo(receiptDto.ItemId, receiptDto.LocationId, receiptDto.Quantity);
                receipt.SetReceiptDateTime(receiptDto.ReceiptDateTime);
                receipt.SetSeller(receiptDto.Seller);

                // 이전 재고 감소
                beforeInventoryItem.Decrease(quantityBefore);

                // 이후 재고 증가
                var afterInventoryItem = await _inventoryUnitOfWork.InventoryItemRepository
                    .FindByItemIdAndLocationIdAsync(receiptDto.ItemId, receiptDto.LocationId);
                if (afterInventoryItem == null)
                {
                    afterInventoryItem = new InventoryItem(receipt.ItemId, receipt.LocationId, receipt.Quantity);
                    await _inventoryUnitOfWork.InventoryItemRepository.AddAsync(afterInventoryItem);
                }
                else
                {
                    afterInventoryItem.Increase(receipt.Quantity);
                }
            }

            await _inventoryUnitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }

}
