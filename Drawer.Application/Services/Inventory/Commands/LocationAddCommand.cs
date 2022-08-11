using Drawer.Application.Config;
using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.Repos;
using Drawer.Domain.Models.Inventory;

namespace Drawer.Application.Services.Inventory.Commands
{
    public record LocationAddCommand(LocationAddCommandModel Location) : ICommand<long>;

    public class LocationAddCommandHandler : ICommandHandler<LocationAddCommand, long>
    {
        private readonly ILocationRepository _locationRepository;

        public LocationAddCommandHandler(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<long> Handle(LocationAddCommand command, CancellationToken cancellationToken)
        {
            var locationDto = command.Location;

            if (await _locationRepository.ExistByName(locationDto.Name))
                throw new AppException($"동일한 이름이 존재합니다. {locationDto.Name}");

            var parentGroup = locationDto.ParentGroupId.HasValue
                ? await _locationRepository.FindByIdAsync(locationDto.ParentGroupId.Value)
                : null;
            var location = new Location(parentGroup, locationDto.Name, locationDto.IsGroup);
            location.SetNote(locationDto.Note);

            await _locationRepository.AddAsync(location);
            await _locationRepository.SaveChangesAsync();

            return location.Id;
        }
    }
}
