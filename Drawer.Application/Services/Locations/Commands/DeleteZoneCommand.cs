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
    /// 구역을 삭제한다.
    /// </summary>
    public record DeleteZoneCommand(long Id) : ICommand<DeleteZoneResult>;

    public record DeleteZoneResult;

    public class DeleteZoneCommandHandler : ICommandHandler<DeleteZoneCommand, DeleteZoneResult>
    {
        private readonly IZoneRepository _zoneRepository;

        public DeleteZoneCommandHandler(IZoneRepository zoneRepository)
        {
            _zoneRepository =zoneRepository;
        }

        public async Task<DeleteZoneResult> Handle(DeleteZoneCommand command, CancellationToken cancellationToken)
        {
            var zone = await _zoneRepository.FindByIdAsync(command.Id);
            if (zone == null)
                throw new EntityNotFoundException<Zone>(command.Id);
            
            _zoneRepository.Remove(zone);
            await _zoneRepository.SaveChangesAsync();
            return new DeleteZoneResult();

        }
    }
}
