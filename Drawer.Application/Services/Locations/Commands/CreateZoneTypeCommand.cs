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
    /// 구역유형을 생성한다.
    /// </summary>
    public record CreateZoneTypeCommand(string Name) : ICommand<CreateZoneTypeResult>;

    public record CreateZoneTypeResult(long Id, string Name);

    public class CreateZoneTypeCommandHandler : ICommandHandler<CreateZoneTypeCommand, CreateZoneTypeResult>
    {
        private readonly IZoneTypeRepository _ZoneTypeRepository;

        public CreateZoneTypeCommandHandler(IZoneTypeRepository zoneTypeRepository)
        {
            _ZoneTypeRepository = zoneTypeRepository;
        }

        public async Task<CreateZoneTypeResult> Handle(CreateZoneTypeCommand command, CancellationToken cancellationToken)
        {
            var zoneType = new ZoneType(command.Name);

            await _ZoneTypeRepository.AddAsync(zoneType);
            await _ZoneTypeRepository.SaveChangesAsync();

            return new CreateZoneTypeResult(zoneType.Id, zoneType.Name);
        }
    }
}
