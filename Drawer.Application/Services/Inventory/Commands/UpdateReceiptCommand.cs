﻿using Drawer.Application.Config;
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
    public record UpdateReceiptCommand(long Id, long ItemId, long LocationId, decimal Quantity, DateTime ReceiptTime, string? Seller) : ICommand<UpdateReceiptResult>;

    public record UpdateReceiptResult;

    public class UpdateReceiptCommandHandler : ICommandHandler<UpdateReceiptCommand, UpdateReceiptResult>
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        private readonly IItemRepository _itemRepository;
        private readonly ILocationRepository _locationRepository;

        public UpdateReceiptCommandHandler(IInventoryUnitOfWork inventoryUnitOfWork,
                                         IItemRepository itemRepository,
                                         ILocationRepository locationRepository)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
            _itemRepository = itemRepository;
            _locationRepository = locationRepository;
        }

        public async Task<UpdateReceiptResult> Handle(UpdateReceiptCommand command, CancellationToken cancellationToken)
        {
            // 1. 품목, 위치가 같은 경우 (동일한 1개의 재고를 수정한다)
            // 2. 품목, 위치가 다른 경우 (다른 2개의 재고를 수정한다)

            var receipt = await _inventoryUnitOfWork.ReceiptRepository
                .FindByIdAsync(command.Id) ?? throw new EntityNotFoundException<Receipt>(command.Id);


            if(command.ItemId ==  receipt.ItemId && command.LocationId == receipt.LocationId)
            {
                // 품목, 위치가 같은 경우 
                // 1. 입고내역 수정
                // 2. 기존 재고 수정

                var quantityDiff = command.Quantity - receipt.Quantity;
                var inventoryItem = await _inventoryUnitOfWork.InventoryItemRepository
                    .FindByItemIdAndLocationIdAsync(receipt.ItemId, receipt.LocationId);
                if (inventoryItem == null || inventoryItem.Quantity + quantityDiff < 0)
                    throw new AppException("재고수량이 부족하여 입고내역을 수정할 수 없습니다");

                receipt.SetQuantity(command.Quantity);
                receipt.SetReceiptTime(command.ReceiptTime);
                receipt.SetSeller(command.Seller);

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
                if (!await _itemRepository.ExistByIdAsync(command.ItemId))
                    throw new EntityNotFoundException<Item>(command.ItemId);
                if (!await _locationRepository.ExistByIdAsync(command.LocationId))
                    throw new EntityNotFoundException<Location>(command.LocationId);

                var quantityBefore = receipt.Quantity;
       
                receipt.SetInventoryInfo(command.ItemId, command.LocationId, command.Quantity);
                receipt.SetReceiptTime(command.ReceiptTime);
                receipt.SetSeller(command.Seller);

                // 이전 재고 감소
                beforeInventoryItem.Decrease(quantityBefore);

                // 이후 재고 증가
                var afterInventoryItem = await _inventoryUnitOfWork.InventoryItemRepository
                    .FindByItemIdAndLocationIdAsync(command.ItemId, command.LocationId);
                if(afterInventoryItem == null)
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
            return new UpdateReceiptResult();
        }
    }

}
