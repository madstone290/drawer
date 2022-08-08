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
    public record CreateIssueCommand(long ItemId, long LocationId, decimal Quantity, DateTime IssueTime, string? Buyer) 
        : ICommand<CreateIssueResult>;

    public record CreateIssueResult(long Id);

    public class CreateIssueCommandHandler : ICommandHandler<CreateIssueCommand, CreateIssueResult>
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        private readonly IItemRepository _itemRepository;
        private readonly ILocationRepository _locationRepository;

        public CreateIssueCommandHandler(IInventoryUnitOfWork inventoryUnitOfWork, 
                                         IItemRepository itemRepository, 
                                         ILocationRepository locationRepository)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
            _itemRepository = itemRepository;
            _locationRepository = locationRepository;
        }

        public async Task<CreateIssueResult> Handle(CreateIssueCommand command, CancellationToken cancellationToken)
        {
            // 출고내역 생성 후 재고 감소

            // 재고확인
            var inventoryItem = await _inventoryUnitOfWork.InventoryItemRepository
                .FindByItemIdAndLocationIdAsync(command.ItemId, command.LocationId);
            if (inventoryItem == null || inventoryItem.Quantity < command.Quantity)
                throw new AppException("재고수량이 부족하여 출고내역을 생성할 수 없습니다");

            // 출고내역 생성
            if (!await _itemRepository.ExistByIdAsync(command.ItemId))
                throw new EntityNotFoundException<Item>(command.ItemId);
            if(!await _locationRepository.ExistByIdAsync(command.LocationId))
                throw new EntityNotFoundException<Location>(command.LocationId);

            var issue = new Issue(command.ItemId, command.LocationId, command.Quantity);
            issue.SetIssueTime(command.IssueTime);
            issue.SetBuyer(command.Buyer);

            await _inventoryUnitOfWork.IssueRepository.AddAsync(issue);

            // 재고 감소
            inventoryItem.Decrease(command.Quantity);


            await _inventoryUnitOfWork.SaveChangesAsync();
            return new CreateIssueResult(issue.Id);
        }
    }
}

