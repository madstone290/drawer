using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.Repos;
using Drawer.Domain.Models.Inventory;

namespace Drawer.Application.Services.Inventory.Commands
{
    public record LocationAddCommand(LocationAddCommandModel Location) : ICommand<long>;

    public class LocationAddCommandHandler : ICommandHandler<LocationAddCommand, long>
    {
        private readonly ILocationRepository _locationRepository;
        private readonly ILocationGroupRepository _groupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LocationAddCommandHandler(ILocationRepository locationRepository, ILocationGroupRepository groupRepository, IUnitOfWork unitOfWork)
        {
            _locationRepository = locationRepository;
            _groupRepository = groupRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<long> Handle(LocationAddCommand command, CancellationToken cancellationToken)
        {
            var locationDto = command.Location;

            if (await _locationRepository.ExistByName(locationDto.Name))
                throw new AppException($"동일한 이름이 존재합니다. {locationDto.Name}");

            var group = await _groupRepository.FindByIdAsync(locationDto.GroupId)
                ?? throw new EntityNotFoundException<LocationGroup>(locationDto.GroupId);

            var location = new Location(group, locationDto.Name);
            location.SetNote(locationDto.Note);

            await _locationRepository.AddAsync(location);
            
            await _unitOfWork.CommitAsync();
            return location.Id;
        }
    }
}
