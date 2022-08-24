using Drawer.Application.Config;
using Drawer.Application.Exceptions;
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
    public record LayoutRemoveCommand(long Id) : ICommand;

    public class LayoutRemoveCommandHandler : ICommandHandler<LayoutRemoveCommand>
    {
        private readonly ILayoutRepository _layoutRepository;

        public LayoutRemoveCommandHandler(ILayoutRepository layoutRepository)
        {
            _layoutRepository = layoutRepository;
        }

        public async Task<Unit> Handle(LayoutRemoveCommand command, CancellationToken cancellationToken)
        {
            var layout = await _layoutRepository.FindByIdAsync(command.Id)
                ?? throw new EntityNotFoundException<Layout>(command.Id);

            _layoutRepository.Remove(layout);

            await _layoutRepository.SaveChangesAsync();
            return Unit.Value;

        }
    }
}
