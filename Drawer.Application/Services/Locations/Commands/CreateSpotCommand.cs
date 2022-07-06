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
    /// <summary>
    /// 자리를 생성한다.
    /// </summary>
    public record CreateSpotCommand(long ZoneId, string Name, string? Note) : ICommand<CreateSpotResult>;

    public record CreateSpotResult(long Id);

    public class CreatePositionCommandHandler : ICommandHandler<CreateSpotCommand, CreateSpotResult>
    {
        private readonly ISpotRepository _spotRepository;
        private readonly IZoneRepository _zoneRepository;

        public CreatePositionCommandHandler(ISpotRepository spotRepository, IZoneRepository zoneRepository)
        {
            _spotRepository = spotRepository;
            _zoneRepository = zoneRepository;
        }

        public async Task<CreateSpotResult> Handle(CreateSpotCommand command, CancellationToken cancellationToken)
        {
            var zone = await _zoneRepository.FindByIdAsync(command.ZoneId) 
                ?? throw new EntityNotFoundException<Zone>(command.ZoneId);
            var spot = new Spot(zone, command.Name);
            spot.SetNote(command.Note);

            await _spotRepository.AddAsync(spot);
            await _spotRepository.SaveChangesAsync();

            return new CreateSpotResult(spot.Id);
        }
    }
}
