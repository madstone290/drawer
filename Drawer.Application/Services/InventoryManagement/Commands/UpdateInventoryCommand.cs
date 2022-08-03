using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.BasicInfo.Repos;
using Drawer.Application.Services.InventoryManagement.Repos;
using Drawer.Application.Services.Items.Repos;
using Drawer.Domain.Models.BasicInfo;
using Drawer.Domain.Models.InventoryManagement;
using Drawer.Domain.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.InventoryManagement.Commands
{
    /// <summary>
    /// 재고를 수정한다.
    /// </summary>
    /// <param name="ItemId">아이템</param>
    /// <param name="LocationId">위치</param>
    /// <param name="QuantityChange">재고 변화량(실수)</param>
    public record UpdateInventoryCommand(long ItemId, long LocationId, decimal QuantityChange) : ICommand<UpdateInventoryResult>;

    public record UpdateInventoryResult;

    public class UpdateInventoryCommandHandler : ICommandHandler<UpdateInventoryCommand, UpdateInventoryResult>
    {
        private readonly IInventoryDetailRepository _inventoryDetailRepository;
        private readonly IItemRepository _itemRepository;
        private readonly ILocationRepository _locationRepository;

        public UpdateInventoryCommandHandler(IInventoryDetailRepository inventoryDetailRepository,
            IItemRepository itemRepository, ILocationRepository locationRepository)
        {
            _inventoryDetailRepository = inventoryDetailRepository;
            _itemRepository = itemRepository;
            _locationRepository = locationRepository;
        }

        public async Task<UpdateInventoryResult> Handle(UpdateInventoryCommand command, CancellationToken cancellationToken)
        {
            var inventoryDetail = await _inventoryDetailRepository.FindByItemIdAndLocationIdAsync(command.ItemId, command.LocationId);
            if (inventoryDetail == null)
            {
                var item = await _itemRepository.FindByIdAsync(command.ItemId)
                    ?? throw new EntityNotFoundException<Item>(command.ItemId);
                var location = await _locationRepository.FindByIdAsync(command.LocationId)
                    ?? throw new EntityNotFoundException<Location>(command.LocationId);

                inventoryDetail = new InventoryDetail(item, location, command.QuantityChange);
                await _inventoryDetailRepository.AddAsync(inventoryDetail);
                await _inventoryDetailRepository.SaveChangesAsync();
            }
            else
            {
                inventoryDetail.Change(command.QuantityChange);
            }

            return new UpdateInventoryResult();
        }
    }
}
