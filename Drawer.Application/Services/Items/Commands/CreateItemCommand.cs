using Drawer.Application.Config;
using Drawer.Application.Services.Items.Repos;
using Drawer.Domain.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Items.Commands
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
            var item = new Item(command.Name);
            item.SetCode(command.Code);
            item.SetNumber(command.Number);
            item.SetSku(command.Sku);
            item.SetMeasurementUnit(command.MeasurementUnit);

            await _itemRepository.AddAsync(item);
            await _itemRepository.SaveChangesAsync();

            return new CreateItemResult(item.Id);
        }
    }
}
