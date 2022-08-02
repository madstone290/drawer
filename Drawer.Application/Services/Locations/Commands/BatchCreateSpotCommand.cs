using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Locations.Repos;
using Drawer.Domain.Models.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Locations.Commands
{
    public record BatchCreateSpotCommand(IList<BatchCreateSpotCommand.Spot> SpotList)
        : ICommand<BatchCreateSpotResult>
    {
        public record Spot(long ZoneId, string Name, string? Note);
    }

    public record BatchCreateSpotResult(IList<long> IdList);

    public class BatchCreateSpotCommandHandler : ICommandHandler<BatchCreateSpotCommand, BatchCreateSpotResult>
    {
        private readonly ISpotRepository _spotRepository;
        private readonly IZoneRepository _zoneRepository;

        public BatchCreateSpotCommandHandler(ISpotRepository spotRepository, IZoneRepository zoneRepository)
        {
            _spotRepository = spotRepository;
            _zoneRepository = zoneRepository;
        }

        public async Task<BatchCreateSpotResult> Handle(BatchCreateSpotCommand command, CancellationToken cancellationToken)
        {
            var spotList = new List<Spot>();
            foreach (var spotDto in command.SpotList)
            {
                var zone = await _zoneRepository.FindByIdAsync(spotDto.ZoneId)
                    ?? throw new EntityNotFoundException<Zone>(spotDto.ZoneId);
                var spot = new Spot(zone, spotDto.Name);

                spot.SetNote(spotDto.Note);

                await _spotRepository.AddAsync(spot);
                spotList.Add(spot);
            }

            await _spotRepository.SaveChangesAsync();

            return new BatchCreateSpotResult(spotList.Select(x => x.Id).ToList());
        }
    }
}

