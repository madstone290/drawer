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

namespace Drawer.Application.Services.Inventory.Commands.IssueCommands
{
    public record DeleteIssueCommand(long Id) : ICommand;

    public class DeleteIssueCommandHandler : ICommandHandler<DeleteIssueCommand>
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;

        public DeleteIssueCommandHandler(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }

        public async Task<Unit> Handle(DeleteIssueCommand command, CancellationToken cancellationToken)
        {
            // 출고내역을 수정하고 재고수량을 증가한다.

            var issue = await _inventoryUnitOfWork.IssueRepository
                .FindByIdAsync(command.Id) ?? throw new EntityNotFoundException<Issue>(command.Id);

            _inventoryUnitOfWork.IssueRepository.Remove(issue);

            var inventoryItem = await _inventoryUnitOfWork.InventoryItemRepository
                .FindByItemIdAndLocationIdAsync(issue.ItemId, issue.LocationId);
            if (inventoryItem == null)
            {
                inventoryItem = new InventoryItem(issue.ItemId, issue.LocationId, issue.Quantity);
                await _inventoryUnitOfWork.InventoryItemRepository.AddAsync(inventoryItem);
            }
            else
            {
                inventoryItem.Increase(issue.Quantity);
            }

            await _inventoryUnitOfWork.SaveChangesAsync();
            return Unit.Value;

        }
    }
}
