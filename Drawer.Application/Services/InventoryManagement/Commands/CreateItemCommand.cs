using Drawer.Application.Config;
using Drawer.Application.Services.InventoryManagement.Repos;
using Drawer.Domain.Models.InventoryManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.InventoryManagement.Commands
{
    /// <summary>
    /// 아이템을 생성한다.
    /// </summary>
    public record CreateItemCommand(string Name, string? Code, string? Number,
        string? Sku, string? MeasurementUnit)
        : ICommand<CreateItemResult>;

    public record CreateItemResult(long Id);

    public class CreateItemCommandHandler : ICommandHandler<CreateItemCommand, CreateItemResult>
    {
        private readonly IItemRepository _itemRepository;

        public CreateItemCommandHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<CreateItemResult> Handle(CreateItemCommand command, CancellationToken cancellationToken)
        {
            if (await _itemRepository.ExistByName(command.Name))
                throw new AppException($"동일한 이름이 존재합니다. {command.Name}");

            var item = new Item(command.Name);
            item.SetCode(command.Code);
            item.SetNumber(command.Number);
            item.SetSku(command.Sku);
            item.SetQuantityUnit(command.MeasurementUnit);

            await _itemRepository.AddAsync(item);
            await _itemRepository.SaveChangesAsync();

            return new CreateItemResult(item.Id);
        }
    }
}
