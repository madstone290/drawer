﻿using Drawer.Application.Config;
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

namespace Drawer.Application.Services.Inventory.Commands
{
    public record IssueUpdateCommand(long Id, IssueCommandModel Issue) : ICommand;

    public class IssueUpdateCommandHandler : ICommandHandler<IssueUpdateCommand>
    {
        private readonly IItemRepository _itemRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly IIssueRepository _issueRepository;
        private readonly IUnitOfWork _unitOfWork;

        public IssueUpdateCommandHandler(IItemRepository itemRepository,
            ILocationRepository locationRepository,
            IInventoryItemRepository inventoryItemRepository,
            IIssueRepository issueRepository,
            IUnitOfWork unitOfWork)
        {
            _itemRepository = itemRepository;
            _locationRepository = locationRepository;
            _inventoryItemRepository = inventoryItemRepository;
            _issueRepository = issueRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(IssueUpdateCommand command, CancellationToken cancellationToken)
        {
            // 1. 품목, 위치가 같은 경우 (동일한 1개의 재고를 수정한다)
            // 2. 품목, 위치가 다른 경우 (다른 2개의 재고를 수정한다)

            var issueId = command.Id;
            var issueDto = command.Issue;

            var issue = await _issueRepository
                .FindByIdAsync(issueId) ?? throw new EntityNotFoundException<Issue>(issueId);


            if (issueDto.ItemId == issue.ItemId && issueDto.LocationId == issue.LocationId)
            {
                // 품목, 위치가 같은 경우 
                // 1. 출고내역 수정
                // 2. 재고 수정

                // 출고내역 수정
                var quantityDiff = issueDto.Quantity - issue.Quantity;
                var inventoryItem = await _inventoryItemRepository
                    .FindByItemIdAndLocationIdAsync(issue.ItemId, issue.LocationId);
                if (inventoryItem == null || inventoryItem.Quantity - quantityDiff < 0)
                    throw new AppException("재고수량이 부족하여 출고내역을 수정할 수 없습니다");

                var quantityBefore = issue.Quantity;

                issue.SetQuantity(issueDto.Quantity);
                issue.SetIssueTime(issueDto.IssueDateTimeLocal);
                issue.SetBuyer(issueDto.Buyer);
                issue.SetNote(issueDto.Note);

                // 재고 수정
                inventoryItem.Increase(quantityBefore);
                inventoryItem.Decrease(issueDto.Quantity);
            }
            else
            {
                // 품목, 위치가 다른 경우 
                // 1. 출고내역 수정
                // 2. 이전 재고 증가
                // 3. 이후 재고 감소

                // 재고수량 확인
                var afterInventoryItem = await _inventoryItemRepository
                    .FindByItemIdAndLocationIdAsync(issueDto.ItemId, issueDto.LocationId);
                if (afterInventoryItem == null || afterInventoryItem.Quantity - issueDto.Quantity < 0)
                    throw new AppException("재고수량이 부족하여 출고내역을 수정할 수 없습니다");

                //  출고내역 생성
                if (!await _itemRepository.ExistByIdAsync(issueDto.ItemId))
                    throw new EntityNotFoundException<Item>(issueDto.ItemId);
                if (!await _locationRepository.ExistByIdAsync(issueDto.LocationId))
                    throw new EntityNotFoundException<Location>(issueDto.LocationId);

                var itemIdBefore = issue.ItemId;
                var locationIdBefore = issue.LocationId;
                var quantityBefore = issue.Quantity;

                issue.SetInventoryInfo(issueDto.ItemId, issueDto.LocationId, issueDto.Quantity);
                issue.SetIssueTime(issueDto.IssueDateTimeLocal);
                issue.SetBuyer(issueDto.Buyer);
                issue.SetNote(issueDto.Note);

                // 이전 재고 증가
                var beforeInventoryItem = await _inventoryItemRepository
                    .FindByItemIdAndLocationIdAsync(itemIdBefore, locationIdBefore);
                if (beforeInventoryItem == null)
                {
                    beforeInventoryItem = new InventoryItem(itemIdBefore, locationIdBefore, quantityBefore);
                    await _inventoryItemRepository.AddAsync(beforeInventoryItem);
                }
                else
                {
                    beforeInventoryItem.Increase(quantityBefore);
                }

                // 이후 재고 감소
                afterInventoryItem.Decrease(issueDto.Quantity);
            }

            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }

}
