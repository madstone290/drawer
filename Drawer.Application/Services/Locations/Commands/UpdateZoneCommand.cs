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
    /// 구역을 수정한다.
    /// </summary>
    public record UpdateZoneCommand(long Id, string Name, long? ZoneTypeId) : ICommand<UpdateZoneResult>;

    public record UpdateZoneResult;

    public class UpdateZoneCommandHandler : ICommandHandler<UpdateZoneCommand, UpdateZoneResult>
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly IZoneTypeRepository _zoneTypeRepository;

        public UpdateZoneCommandHandler(IZoneRepository zoneRepository, IZoneTypeRepository zoneTypeRepository)
        {
            _zoneRepository = zoneRepository;
            _zoneTypeRepository = zoneTypeRepository;
        }

        public async Task<UpdateZoneResult> Handle(UpdateZoneCommand command, CancellationToken cancellationToken)
        {
            var zone = await _zoneRepository.FindByIdAsync(command.Id);
            if (zone == null)
                throw new EntityNotFoundException<Zone>(command.Id);

            if (command.ZoneTypeId == null)
            {
                zone.SetZoneType(null);
            }
            else
            {
                var zoneType = await _zoneTypeRepository.FindByIdAsync(command.ZoneTypeId.Value);
                zone.SetZoneType(zoneType);
            }

            zone.SetName(command.Name);
            await _zoneRepository.SaveChangesAsync();
            return new UpdateZoneResult();
        }
    }

}
