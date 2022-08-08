using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Inventory.Repos;
using Drawer.Domain.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.Commands
{
    public record UpdateLocationCommand(long Id, string Name, string? Note) : ICommand<UpdateLocationResult>;

    public record UpdateLocationResult;

    public class UpdateLocationCommandHandler : ICommandHandler<UpdateLocationCommand, UpdateLocationResult>
    {
        private readonly ILocationRepository _locationRepository;

        public UpdateLocationCommandHandler(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<UpdateLocationResult> Handle(UpdateLocationCommand command, CancellationToken cancellationToken)
        {
            var location = await _locationRepository.FindByIdAsync(command.Id)
                ?? throw new EntityNotFoundException<Location>(command.Id);
            
            if (await _locationRepository.ExistByName(command.Name))
                throw new AppException($"동일한 이름이 존재합니다. {command.Name}");

            location.SetName(command.Name);
            location.SetNote(command.Note);

            await _locationRepository.SaveChangesAsync();
            return new UpdateLocationResult();
        }
    }

}
