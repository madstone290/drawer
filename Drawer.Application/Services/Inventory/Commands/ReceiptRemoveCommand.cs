using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Inventory.Repos;
using Drawer.Domain.Models.Inventory;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.Commands
{
    public record ReceiptRemoveCommand(long Id) : ICommand;

    public class ReceiptRemoveCommandHandler : ICommandHandler<ReceiptRemoveCommand>
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReceiptRemoveCommandHandler(IInventoryItemRepository inventoryItemRepository, IReceiptRepository receiptRepository, IUnitOfWork unitOfWork)
        {
            _inventoryItemRepository = inventoryItemRepository;
            _receiptRepository = receiptRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(ReceiptRemoveCommand command, CancellationToken cancellationToken)
        {
            // 입고내역을 수정하고 재고수량을 감소한다.

            var receipt = await _receiptRepository
                .FindByIdAsync(command.Id) ?? throw new EntityNotFoundException<Receipt>(command.Id);

            // 재고수량 확인. 입고 위치의 아이템 재고수량이 입고수량보다 적은 경우 삭제가 불가능
            var inventoryItem = await _inventoryItemRepository
                .FindByItemIdAndLocationIdAsync(receipt.ItemId, receipt.LocationId);
            if (inventoryItem == null || inventoryItem.Quantity < receipt.Quantity)
                throw new AppException("재고수량이 부족하여 입고내역을 삭제할 수 없습니다");

            _receiptRepository.Remove(receipt);
            inventoryItem.Decrease(receipt.Quantity);

            await _unitOfWork.CommitAsync();
            return Unit.Value;

        }
    }
}
