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

namespace Drawer.Application.Services.Inventory.Commands.LocationCommands
{
    public record UpdateLocationCommand(long Id, LocationUpdateCommandModel Location) : ICommand;

    public class UpdateLocationCommandHandler : ICommandHandler<UpdateLocationCommand>
    {
        private readonly ILocationRepository _locationRepository;

        public UpdateLocationCommandHandler(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<Unit> Handle(UpdateLocationCommand command, CancellationToken cancellationToken)
        {
            var locationId = command.Id;
            var locationDto = command.Location;

            var location = await _locationRepository.FindByIdAsync(locationId)
                ?? throw new EntityNotFoundException<Location>(locationId);

            if (await _locationRepository.ExistByName(locationDto.Name))
                throw new AppException($"동일한 이름이 존재합니다. {locationDto.Name}");

            location.SetName(locationDto.Name);
            location.SetNote(locationDto.Note);

            await _locationRepository.SaveChangesAsync();
            return Unit.Value;
        }
    }

}
