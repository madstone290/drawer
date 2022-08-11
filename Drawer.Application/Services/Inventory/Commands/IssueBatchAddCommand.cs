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
    public record IssueBatchAddCommand(List<IssueCommandModel> IssueList) : ICommand<List<long>>;

    public class IssueBatchAddCommandHandler : ICommandHandler<IssueBatchAddCommand, List<long>>
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        private readonly IItemRepository _itemRepository;
        private readonly ILocationRepository _locationRepository;

        public IssueBatchAddCommandHandler(IInventoryUnitOfWork inventoryUnitOfWork,
                                           IItemRepository itemRepository,
                                           ILocationRepository locationRepository)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
            _itemRepository = itemRepository;
            _locationRepository = locationRepository;
        }

        public async Task<List<long>> Handle(IssueBatchAddCommand command, CancellationToken cancellationToken)
        {
            // 출고내역 생성 후 재고 감소

            var issueList = new List<Issue>();
            foreach (var issueDto in command.IssueList)
            {
                // 재고확인
                var inventoryItem = await _inventoryUnitOfWork.InventoryItemRepository
                    .FindByItemIdAndLocationIdAsync(issueDto.ItemId, issueDto.LocationId);
                if (inventoryItem == null || inventoryItem.Quantity < issueDto.Quantity)
                    throw new AppException("재고수량이 부족하여 출고내역을 생성할 수 없습니다");

                // 출고내역 생성
                if (!await _itemRepository.ExistByIdAsync(issueDto.ItemId))
                    throw new EntityNotFoundException<Item>(issueDto.ItemId);
                if (!await _locationRepository.ExistByIdAsync(issueDto.LocationId))
                    throw new EntityNotFoundException<Location>(issueDto.LocationId);

                var transactionNumber = Guid.NewGuid().ToString();
                var issue = new Issue(transactionNumber, issueDto.ItemId, issueDto.LocationId, issueDto.Quantity);
                issue.SetIssueTime(issueDto.IssueDateTimeLocal);
                issue.SetBuyer(issueDto.Buyer);
                issue.SetNote(issueDto.Note);
                issueList.Add(issue);

                await _inventoryUnitOfWork.IssueRepository.AddAsync(issue);

                // 재고 감소
                inventoryItem.Decrease(issueDto.Quantity);
            }

            await _inventoryUnitOfWork.SaveChangesAsync();
            return issueList.Select(x => x.Id).ToList();
        }
    }
}
