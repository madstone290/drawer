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
    public record LayoutEditCommand(LayoutEditCommandModel Layout) : ICommand;

    public class LayoutUpdateCommandHandler : ICommandHandler<LayoutEditCommand>
    {
        private readonly ILayoutRepository _layoutRepository;
        private readonly ILocationGroupRepository _groupRepository;
        private readonly ILocationRepository _locationRepository;

        public LayoutUpdateCommandHandler(ILayoutRepository layoutRepository, ILocationGroupRepository groupRepository, ILocationRepository locationRepository)
        {
            _layoutRepository = layoutRepository;
            _groupRepository = groupRepository;
            _locationRepository = locationRepository;
        }

        public async Task<Unit> Handle(LayoutEditCommand command, CancellationToken cancellationToken)
        {
            var layoutDto = command.Layout;
            var layout = await _layoutRepository.FindByLocationId(layoutDto.LocationGroupId);
            if(layout == null)
                layout = await CreateLayout(layoutDto);

            foreach(var locationId in layoutDto.ItemList.SelectMany(x=> x.ConnectedLocations))
            {
                if (await _locationRepository.ExistByIdAsync(locationId) == false)
                    throw new EntityNotFoundException<Location>(locationId);
            }

            layout.Update(layoutDto.ItemList);

            await _layoutRepository.SaveChangesAsync();
            return Unit.Value;
        }

        async Task<Layout> CreateLayout(LayoutEditCommandModel layoutDto)
        {
            // validate location Id and Type
            var group = await _groupRepository.FindByIdAsync(layoutDto.LocationGroupId)
                ?? throw new EntityNotFoundException<LocationGroup>(layoutDto.LocationGroupId);
            if (!group.IsRoot)
                throw new AppException("루트그룹만 레이아웃을 가질 수 있습니다");

            var layout = new Layout(layoutDto.LocationGroupId);

            await _layoutRepository.AddAsync(layout);

            return layout;
        }
    }
}
