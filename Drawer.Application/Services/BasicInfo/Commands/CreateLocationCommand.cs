using Drawer.Application.Config;
using Drawer.Application.Services.BasicInfo.Repos;
using Drawer.Domain.Models.BasicInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.BasicInfo.Commands
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
