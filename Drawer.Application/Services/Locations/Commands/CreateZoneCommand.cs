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
    /// 구역을 생성한다.
    /// </summary>
    public record CreateZoneCommand(long WorkPlaceId, string Name, string? Note) : ICommand<CreateZoneResult>;

    public record CreateZoneResult(long Id);

    public class CreateZoneCommandHandler : ICommandHandler<CreateZoneCommand, CreateZoneResult>
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly IWorkPlaceRepository _workPlaceRepository;

        public CreateZoneCommandHandler(IZoneRepository zoneRepository, IWorkPlaceRepository workPlaceRepository)
        {
            _zoneRepository = zoneRepository;
            _workPlaceRepository = workPlaceRepository;
        }

        public async Task<CreateZoneResult> Handle(CreateZoneCommand command, CancellationToken cancellationToken)
        {
            var workPlace = await _workPlaceRepository.FindByIdAsync(command.WorkPlaceId)
                ?? throw new EntityNotFoundException<WorkPlace>(command.WorkPlaceId);

            var zone = new Zone(workPlace, command.Name);
            zone.SetNote(command.Note);

            await _zoneRepository.AddAsync(zone);
            await _zoneRepository.SaveChangesAsync();

            return new CreateZoneResult(zone.Id);
        }
    }
}
