using Drawer.Application.Config;
using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.Repos;
using Drawer.Domain.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.Commands.ItemCommands
{
    /// <summary>
    /// 아이템을 생성한다.
    /// </summary>
    public record CreateItemCommand(ItemAddUpdateCommandModel Item) : ICommand<long>;

    public class CreateItemCommandHandler : ICommandHandler<CreateItemCommand, long>
    {
        private readonly IItemRepository _itemRepository;

        public CreateItemCommandHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<long> Handle(CreateItemCommand command, CancellationToken cancellationToken)
        {
            var itemDto = command.Item;

            if (await _itemRepository.ExistByName(itemDto.Name))
                throw new AppException($"동일한 이름이 존재합니다. {itemDto.Name}");

            var item = new Item(itemDto.Name);
            item.SetCode(itemDto.Code);
            item.SetNumber(itemDto.Number);
            item.SetSku(itemDto.Sku);
            item.SetQuantityUnit(itemDto.QuantityUnit);

            await _itemRepository.AddAsync(item);
            await _itemRepository.SaveChangesAsync();

            return item.Id;
        }
    }
}
