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
    public record CreateLocationCommand(long? UpperLocationId, string Name, string? Note) : ICommand<CreateLocationResult>;

    public record CreateLocationResult(long Id);

    public class CreateLocationCommandHandler : ICommandHandler<CreateLocationCommand, CreateLocationResult>
    {
        private readonly ILocationRepository _locationRepository;

        public CreateLocationCommandHandler(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<CreateLocationResult> Handle(CreateLocationCommand command, CancellationToken cancellationToken)
        {
            if (await _locationRepository.ExistByName(command.Name))
                throw new AppException($"위치 이름 중복 {command.Name}");

            var upperLocation = command.UpperLocationId.HasValue
                ? await _locationRepository.FindByIdAsync(command.UpperLocationId.Value)
                : null;
            var location = new Location(upperLocation, command.Name);
            location.SetNote(command.Note);

            await _locationRepository.AddAsync(location);
            await _locationRepository.SaveChangesAsync();

            return new CreateLocationResult(location.Id);
        }
    }
}
