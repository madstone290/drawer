using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.Repos;
using Drawer.Domain.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.Commands
{
    public record LayoutAddCommand(LayoutAddCommandModel Layout) : ICommand<long>;

    public class LayoutAddCommandHandler : ICommandHandler<LayoutAddCommand, long>
    {
        private readonly ILayoutRepository _layoutRepository;
        private readonly ILocationRepository _locationRepository;

        public LayoutAddCommandHandler(ILayoutRepository layoutRepository, ILocationRepository locationRepository)
        {
            _layoutRepository = layoutRepository;
            _locationRepository = locationRepository;
        }

        public async Task<long> Handle(LayoutAddCommand command, CancellationToken cancellationToken)
        {
            var layoutDto = command.Layout;

            // validate location Id and Type
            var location = await _locationRepository.FindByIdAsync(layoutDto.LocationId);
            if (location == null)
                throw new EntityNotFoundException<Location>(layoutDto.LocationId);
            if (!location.IsRootGroup)
                throw new AppException("루트그룹만 레이아웃을 가질 수 있습니다");

            var layout = new Layout(layoutDto.LocationId);

            await _layoutRepository.AddAsync(layout);
            await _layoutRepository.SaveChangesAsync();

            return layout.Id;
        }
    }
}
