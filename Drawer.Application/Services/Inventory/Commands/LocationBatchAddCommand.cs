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
    public record LocationBatchAddCommand(List<LocationAddCommandModel> LocationList) : ICommand<List<long>>;

    public class LocationBatchAddCommandHandler : ICommandHandler<LocationBatchAddCommand, List<long>>
    {
        private readonly ILocationRepository _locationRepository;
        private readonly ILocationGroupRepository _groupRepository;

        public LocationBatchAddCommandHandler(ILocationRepository locationRepository, ILocationGroupRepository groupRepository)
        {
            _locationRepository = locationRepository;
            _groupRepository = groupRepository;
        }
        public async Task<List<long>> Handle(LocationBatchAddCommand command, CancellationToken cancellationToken)
        {
            var locationList = new List<Location>();
            foreach (var locationDto in command.LocationList)
            {
                if (await _locationRepository.ExistByName(locationDto.Name))
                    throw new AppException($"동일한 이름이 존재합니다. {locationDto.Name}");

                var group = await _groupRepository.FindByIdAsync(locationDto.GroupId)
                    ?? throw new EntityNotFoundException<LocationGroup>(locationDto.GroupId);

                var location = new Location(group, locationDto.Name);
                location.SetNote(locationDto.Note);

                await _locationRepository.AddAsync(location);
                locationList.Add(location);
            }

            await _locationRepository.SaveChangesAsync();

            return locationList.Select(x => x.Id).ToList();
        }
    }
}
