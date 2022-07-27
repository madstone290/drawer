using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Items.Repos;
using Drawer.Domain.Models.Items;
using Drawer.Domain.Models.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Items.Commands
{
    /// <summary>
    /// 구역유형을 수정한다.
    /// </summary>
    public record UpdateItemCommand(long Id, string Name, string? Code, string? Number,
        string? Sku, string? MeasurementUnit) : ICommand<UpdateItemResult>;

    public record UpdateItemResult;

    public class UpdateItemCommandHandler : ICommandHandler<UpdateItemCommand, UpdateItemResult>
    {
        private readonly IItemRepository _itemRepository;

        public UpdateItemCommandHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<UpdateItemResult> Handle(UpdateItemCommand command, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.FindByIdAsync(command.Id)
                ?? throw new EntityNotFoundException<Item>(command.Id);

            item.SetName(command.Name);
            item.SetNumber(command.Name);
            item.SetCode(command.Code);
            item.SetNumber(command.Number);
            item.SetSku(command.Sku);
            item.SetQuantityUnit(command.MeasurementUnit);

            await _itemRepository.SaveChangesAsync();
            return new UpdateItemResult();
        }
    }

}
