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
    public record DeleteReceiptCommand(long Id) : ICommand<DeleteReceiptResult>;

    public record DeleteReceiptResult;

    public class DeleteReceiptCommandHandler : ICommandHandler<DeleteReceiptCommand, DeleteReceiptResult>
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;

        public DeleteReceiptCommandHandler(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }

        public async Task<DeleteReceiptResult> Handle(DeleteReceiptCommand command, CancellationToken cancellationToken)
        {
            // 입고내역을 수정하고 재고수량을 감소한다.

            var receipt = await _inventoryUnitOfWork.ReceiptRepository
                .FindByIdAsync(command.Id) ?? throw new EntityNotFoundException<Receipt>(command.Id);

            // 재고수량 확인. 입고 위치의 아이템 재고수량이 입고수량보다 적은 경우 삭제가 불가능
            var inventoryItem = await _inventoryUnitOfWork.InventoryItemRepository
                .FindByItemIdAndLocationIdAsync(receipt.ItemId, receipt.LocationId);
            if (inventoryItem == null || inventoryItem.Quantity < receipt.Quantity)
                throw new AppException("재고수량이 부족하여 입고내역을 삭제할 수 없습니다");

            _inventoryUnitOfWork.ReceiptRepository.Remove(receipt);
            inventoryItem.Decrease(receipt.Quantity);

            await _inventoryUnitOfWork.SaveChangesAsync();
            return new DeleteReceiptResult();

        }
    }
}
