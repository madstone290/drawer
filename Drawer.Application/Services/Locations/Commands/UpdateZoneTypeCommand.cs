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
    /// 구역유형을 수정한다.
    /// </summary>
    public record UpdateZoneTypeCommand(long Id, string Name) : ICommand<UpdateZoneTypeResult>;

    public record UpdateZoneTypeResult;

    public class UpdateZoneTypeCommandHandler : ICommandHandler<UpdateZoneTypeCommand, UpdateZoneTypeResult>
    {
        private readonly IZoneTypeRepository _zoneTypeRepository;

        public UpdateZoneTypeCommandHandler(IZoneTypeRepository zoneTypeRepository)
        {
            _zoneTypeRepository = zoneTypeRepository;
        }

        public async Task<UpdateZoneTypeResult> Handle(UpdateZoneTypeCommand command, CancellationToken cancellationToken)
        {
            var zoneType = await _zoneTypeRepository.FindByIdAsync(command.Id);
            if (zoneType == null)
                throw new EntityNotFoundException<ZoneType>(command.Id);

            zoneType.SetName(command.Name);
            await _zoneTypeRepository.SaveChangesAsync();
            return new UpdateZoneTypeResult();
        }
    }

}
