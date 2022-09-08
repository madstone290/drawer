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
    public record ReceiptBatchAddCommand(List<ReceiptCommandModel> ReceiptList) : ICommand<List<long>>;

    public class ReceiptBatchAddCommandHandler : ICommandHandler<ReceiptBatchAddCommand, List<long>>
    {
        private readonly IItemRepository _itemRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReceiptBatchAddCommandHandler(IItemRepository itemRepository,
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

        public async Task<List<long>> Handle(ReceiptBatchAddCommand command, CancellationToken cancellationToken)
        {
            // 입고내역 생성 후 재고 증가

            var receiptList = new List<Receipt>();
            foreach (var receiptDto in command.ReceiptList)
            {
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
                receiptList.Add(receipt);

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
            }

            await _unitOfWork.CommitAsync();
            return receiptList.Select(x => x.Id).ToList();
        }
    }
}
