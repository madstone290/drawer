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
    /// 구역유형을 삭제한다.
    /// </summary>
    public record DeleteZoneTypeCommand(long Id) : ICommand<DeleteZoneTypeResult>;

    public record DeleteZoneTypeResult;

    public class DeleteZoneTypeCommandHandler : ICommandHandler<DeleteZoneTypeCommand, DeleteZoneTypeResult>
    {
        private readonly IZoneTypeRepository _zoneTypeRepository;

        public DeleteZoneTypeCommandHandler(IZoneTypeRepository zoneTypeRepository)
        {
            _zoneTypeRepository = zoneTypeRepository;
        }

        public async Task<DeleteZoneTypeResult> Handle(DeleteZoneTypeCommand command, CancellationToken cancellationToken)
        {
            var zoneType = await _zoneTypeRepository.FindByIdAsync(command.Id);
            if (zoneType == null)
                throw new EntityNotFoundException<ZoneType>(command.Id);
            
            _zoneTypeRepository.Remove(zoneType);
            await _zoneTypeRepository.SaveChangesAsync();
            return new DeleteZoneTypeResult();

        }
    }
}
