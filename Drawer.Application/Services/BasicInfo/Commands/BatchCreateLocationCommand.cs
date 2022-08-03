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
    public record BatchCreateLocationCommand(IList<BatchCreateLocationCommand.Location> LocationList)
        : ICommand<BatchCreateLocationResult>
    {
        public record Location(long? UpperLocationId, string Name, string? Note);
    }

    public record BatchCreateLocationResult(IList<long> IdList);

    public class BatchCreateLocationCommandHandler : ICommandHandler<BatchCreateLocationCommand, BatchCreateLocationResult>
    {
        private readonly ILocationRepository _locationRepository;

        public BatchCreateLocationCommandHandler(ILocationRepository LocationRepository)
        {
            _locationRepository = LocationRepository;
        }

        public async Task<BatchCreateLocationResult> Handle(BatchCreateLocationCommand command, CancellationToken cancellationToken)
        {
            var locationList = new List<Location>();
            foreach (var locationDto in command.LocationList)
            {
                var upperLocation = locationDto.UpperLocationId.HasValue
                    ? await _locationRepository.FindByIdAsync(locationDto.UpperLocationId.Value)
                    : null;
                var location = new Location(upperLocation, locationDto.Name);
                location.SetNote(locationDto.Note);

                await _locationRepository.AddAsync(location);
                locationList.Add(location);
            }

            await _locationRepository.SaveChangesAsync();

            return new BatchCreateLocationResult(locationList.Select(x => x.Id).ToList());
        }
    }
}
