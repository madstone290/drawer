using Drawer.Application.Config;
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
    public record ItemUpdateCommand(long Id, ItemCommandModel Item) : ICommand;

    public class ItemUpdateCommandHandler : ICommandHandler<ItemUpdateCommand>
    {
        private readonly IItemRepository _itemRepository;

        public ItemUpdateCommandHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<Unit> Handle(ItemUpdateCommand command1, CancellationToken cancellationToken)
        {
            var itemId = command1.Id;
            var itemDto = command1.Item;

            var item = await _itemRepository.FindByIdAsync(itemId)
                ?? throw new EntityNotFoundException<Item>(itemId);

            item.SetName(itemDto.Name);
            item.SetNumber(itemDto.Name);
            item.SetCode(itemDto.Code);
            item.SetNumber(itemDto.Number);
            item.SetSku(itemDto.Sku);
            item.SetQuantityUnit(itemDto.QuantityUnit);

            await _itemRepository.SaveChangesAsync();
            return Unit.Value;
        }
    }

}
