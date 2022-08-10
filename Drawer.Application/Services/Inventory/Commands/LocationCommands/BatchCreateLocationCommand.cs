using Drawer.Application.Config;
using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.Repos;
using Drawer.Domain.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.Commands.LocationCommands
{
    public record BatchCreateLocationCommand(List<LocationAddCommandModel> LocationList) : ICommand<List<long>>;

    public class BatchCreateLocationCommandHandler : ICommandHandler<BatchCreateLocationCommand, List<long>>
    {
        private readonly ILocationRepository _locationRepository;

        public BatchCreateLocationCommandHandler(ILocationRepository LocationRepository)
        {
            _locationRepository = LocationRepository;
        }

        public async Task<List<long>> Handle(BatchCreateLocationCommand command, CancellationToken cancellationToken)
        {
            var locationList = new List<Location>();
            foreach (var locationDto in command.LocationList)
            {
                var parentGroup = locationDto.ParentGroupId.HasValue
                    ? await _locationRepository.FindByIdAsync(locationDto.ParentGroupId.Value)
                    : null;

                if (await _locationRepository.ExistByName(locationDto.Name))
                    throw new AppException($"동일한 이름이 존재합니다. {locationDto.Name}");

                var location = new Location(parentGroup, locationDto.Name, locationDto.IsGroup);
                location.SetNote(locationDto.Note);

                await _locationRepository.AddAsync(location);
                locationList.Add(location);
            }

            await _locationRepository.SaveChangesAsync();

            return locationList.Select(x => x.Id).ToList();
        }
    }
}
