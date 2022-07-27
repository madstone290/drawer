using Drawer.Application.Config;
using Drawer.Application.Services.Items.Repos;
using Drawer.Domain.Models.Items;

namespace Drawer.Application.Services.Items.Commands
{
    /// <summary>
    /// 아이템을 생성한다.
    /// </summary>
    public record BatchCreateItemCommand(IList<BatchCreateItemCommand.Item> ItemList)
        : ICommand<BatchCreateItemResult>
    {
        public record Item(string Name, string? Code, string? Number, string? Sku, string? QuantityUnit);
    }

    public record BatchCreateItemResult(IList<long> IdList);

    public class BatchCreateItemCommandHandler : ICommandHandler<BatchCreateItemCommand, BatchCreateItemResult>
    {
        private readonly IItemRepository _itemRepository;

        public BatchCreateItemCommandHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<BatchCreateItemResult> Handle(BatchCreateItemCommand command, CancellationToken cancellationToken)
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

            return new BatchCreateItemResult(itemList.Select(x=> x.Id).ToList());
        }
    }
}
