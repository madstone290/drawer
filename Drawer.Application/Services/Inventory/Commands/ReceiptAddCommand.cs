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

namespace Drawer.Application.Services.Inventory.Commands
{
    /// <summary>
    /// 입고를 생성하고 입고ID를 반환한다.
    /// </summary>
    public record ReceiptAddCommand(ReceiptCommandModel Receipt) : ICommand<long>;

    public class ReceiptAddCommandHandler : ICommandHandler<ReceiptAddCommand, long>
    {
        private readonly IItemRepository _itemRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReceiptAddCommandHandler(IItemRepository itemRepository,
            ILocationRepository locationRepository,
            IInventoryItemRepository inventoryItemRepository,
            IReceiptRepository receiptRepository,
            IUnitOfWork unitOfWork)
        {
            _itemRepository = itemRepository;
            _locationRepository = locationRepository;
            _inventoryItemRepository = inventoryItemRepository;
            _receiptRepository = receiptRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<long> Handle(ReceiptAddCommand command, CancellationToken cancellationToken)
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
            receipt.SetReceiptDateTime(receiptDto.ReceiptDateTimeLocal);
            receipt.SetSeller(receiptDto.Seller);
            receipt.SetNote(receiptDto.Note);

            await _receiptRepository.AddAsync(receipt);

            // 재고 증가
            var inventoryItem = await _inventoryItemRepository
               .FindByItemIdAndLocationIdAsync(receiptDto.ItemId, receiptDto.LocationId);
            if (inventoryItem == null)
            {
                inventoryItem = new InventoryItem(receiptDto.ItemId, receiptDto.LocationId, receiptDto.Quantity);
                await _inventoryItemRepository.AddAsync(inventoryItem);
            }
            else
            {
                inventoryItem.Increase(receiptDto.Quantity);
            }

            await _unitOfWork.CommitAsync();
            return receipt.Id;
        }
    }
}

