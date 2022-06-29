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
    /// 위치을 생성한다.
    /// </summary>
    public record CreatePositionCommand(long ZoneId, string Name) : ICommand<CreatePositionResult>;

    public record CreatePositionResult(long Id, long ZoneId, string Name);

    public class CreatePositionCommandHandler : ICommandHandler<CreatePositionCommand, CreatePositionResult>
    {
        private readonly IPositionRepository _positionRepository;
        private readonly IZoneRepository _zoneRepository;

        public CreatePositionCommandHandler(IPositionRepository positionRepository, IZoneRepository zoneRepository)
        {
            _positionRepository = positionRepository;
            _zoneRepository = zoneRepository;
        }

        public async Task<CreatePositionResult> Handle(CreatePositionCommand command, CancellationToken cancellationToken)
        {
            var zone = await _zoneRepository.FindByIdAsync(command.ZoneId) 
                ?? throw new EntityNotFoundException<Zone>(command.ZoneId);
            var position = new Position(zone, command.Name);

            await _positionRepository.AddAsync(position);
            await _positionRepository.SaveChangesAsync();

            return new CreatePositionResult(position.Id, position.ZoneId, position.Name);
        }
    }
}
