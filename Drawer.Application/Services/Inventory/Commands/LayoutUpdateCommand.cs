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
    public record LayoutUpdateCommand(long Id, LayoutUpdateCommandModel Layout) : ICommand;

    public class LayoutUpdateCommandHandler : ICommandHandler<LayoutUpdateCommand>
    {
        private readonly ILayoutRepository _layoutRepository;
        private readonly ILocationRepository _locationRepository;

        public LayoutUpdateCommandHandler(ILayoutRepository layoutRepository, ILocationRepository locationRepository)
        {
            _layoutRepository = layoutRepository;
            _locationRepository = locationRepository;
        }

        public async Task<Unit> Handle(LayoutUpdateCommand command, CancellationToken cancellationToken)
        {
            var layout = await _layoutRepository.FindByIdAsync(command.Id);
            if(layout == null)
                throw new EntityNotFoundException<Layout>(command.Id);

            var layoutDto = command.Layout;
            foreach(var locationId in layoutDto.ItemList.SelectMany(x=> x.ConnectedLocations))
            {
                if (await _locationRepository.ExistByIdAsync(locationId) == false)
                    throw new EntityNotFoundException<Location>(locationId);
            }

            layout.Update(layoutDto.ItemList);

            await _layoutRepository.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
