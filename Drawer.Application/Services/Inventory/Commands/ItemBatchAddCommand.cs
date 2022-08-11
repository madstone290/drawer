using Drawer.Application.Config;
using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.Repos;
using Drawer.Domain.Models.Inventory;

namespace Drawer.Application.Services.Inventory.Commands
{
    /// <summary>
    /// 아이템을 생성한다.
    /// </summary>
    public record ItemBatchAddCommand(List<ItemCommandModel> ItemList) : ICommand<List<long>>;

    public class ItemBatchAddCommandHandler : ICommandHandler<ItemBatchAddCommand, List<long>>
    {
        private readonly IItemRepository _itemRepository;

        public ItemBatchAddCommandHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<List<long>> Handle(ItemBatchAddCommand command, CancellationToken cancellationToken)
        {
            var itemList = new List<Item>();
            foreach (var itemDto in command.ItemList)
            {
                var item = new Item(itemDto.Name);
                item.SetCode(itemDto.Code);
                item.SetNumber(itemDto.Number);
                item.SetSku(itemDto.Sku);
                item.SetQuantityUnit(itemDto.QuantityUnit);

                await _itemRepository.AddAsync(item);
                itemList.Add(item);
            }

            await _itemRepository.SaveChangesAsync();

            return itemList.Select(x => x.Id).ToList();
        }
    }
}
