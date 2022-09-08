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
    public record IssueRemoveCommand(long Id) : ICommand;

    public class IssueRemoveCommandHandler : ICommandHandler<IssueRemoveCommand>
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly IIssueRepository _issueRepository;
        private readonly IUnitOfWork _unitOfWork;

        public IssueRemoveCommandHandler(IInventoryItemRepository inventoryItemRepository, IIssueRepository issueRepository, IUnitOfWork unitOfWork)
        {
            _inventoryItemRepository = inventoryItemRepository;
            _issueRepository = issueRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(IssueRemoveCommand command, CancellationToken cancellationToken)
        {
            // 출고내역을 수정하고 재고수량을 증가한다.

            var issue = await _issueRepository
                .FindByIdAsync(command.Id) ?? throw new EntityNotFoundException<Issue>(command.Id);

            _issueRepository.Remove(issue);

            var inventoryItem = await _inventoryItemRepository
                .FindByItemIdAndLocationIdAsync(issue.ItemId, issue.LocationId);
            if (inventoryItem == null)
            {
                inventoryItem = new InventoryItem(issue.ItemId, issue.LocationId, issue.Quantity);
                await _inventoryItemRepository.AddAsync(inventoryItem);
            }
            else
            {
                inventoryItem.Increase(issue.Quantity);
            }

            await _unitOfWork.CommitAsync();
            return Unit.Value;

        }
    }
}
