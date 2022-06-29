using Drawer.Application.Config;
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
    public record CreateZoneCommand(string Name, long? ZoneTypeId) : ICommand<CreateZoneResult>;

    public record CreateZoneResult(long Id, string Name);

    public class CreateZoneCommandHandler : ICommandHandler<CreateZoneCommand, CreateZoneResult>
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly IZoneTypeRepository _zoneTypeRepository;

        public CreateZoneCommandHandler(IZoneRepository zoneRepository, IZoneTypeRepository zoneTypeRepository)
        {
            _zoneRepository = zoneRepository;
            _zoneTypeRepository = zoneTypeRepository;
        }

        public async Task<CreateZoneResult> Handle(CreateZoneCommand command, CancellationToken cancellationToken)
        {
            var zone = new Zone(command.Name);

            if (command.ZoneTypeId != null)
            {
                var zoneType = await _zoneTypeRepository.FindByIdAsync(command.ZoneTypeId.Value);
                zone.SetZoneType(zoneType);
            }

            await _zoneRepository.AddAsync(zone);
            await _zoneRepository.SaveChangesAsync();

            return new CreateZoneResult(zone.Id, zone.Name);
        }
    }
}
