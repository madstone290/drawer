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
    public record UpdateZoneCommand(long Id, string Name, string? Note) : ICommand<UpdateZoneResult>;

    public record UpdateZoneResult;

    public class UpdateZoneCommandHandler : ICommandHandler<UpdateZoneCommand, UpdateZoneResult>
    {
        private readonly IZoneRepository _zoneRepository;

        public UpdateZoneCommandHandler(IZoneRepository zoneRepository)
        {
            _zoneRepository = zoneRepository;
        }

        public async Task<UpdateZoneResult> Handle(UpdateZoneCommand command, CancellationToken cancellationToken)
        {
            var zone = await _zoneRepository.FindByIdAsync(command.Id);
            if (zone == null)
                throw new EntityNotFoundException<Zone>(command.Id);

            zone.SetName(command.Name);
            zone.SetNote(command.Note);

            await _zoneRepository.SaveChangesAsync();
            return new UpdateZoneResult();
        }
    }

}
