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
    public record BatchCreateZoneCommand(IList<BatchCreateZoneCommand.Zone> ZoneList)
        : ICommand<BatchCreateZoneResult>
    {
        public record Zone(long WorkplaceId, string Name, string? Note);
    }

    public record BatchCreateZoneResult(IList<long> IdList);

    public class BatchCreateZoneCommandHandler : ICommandHandler<BatchCreateZoneCommand, BatchCreateZoneResult>
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly IWorkplaceRepository _workplaceRepository;

        public BatchCreateZoneCommandHandler(IZoneRepository ZoneRepository, IWorkplaceRepository workplaceRepository)
        {
            _zoneRepository = ZoneRepository;
            _workplaceRepository = workplaceRepository;
        }

        public async Task<BatchCreateZoneResult> Handle(BatchCreateZoneCommand command, CancellationToken cancellationToken)
        {
            var zoneList = new List<Zone>();
            foreach (var zoneDto in command.ZoneList)
            {
                var workplace = await _workplaceRepository.FindByIdAsync(zoneDto.WorkplaceId)
                    ?? throw new EntityNotFoundException<Workplace>(zoneDto.WorkplaceId);
                var zone = new Zone(workplace, zoneDto.Name);

                zone.SetNote(zoneDto.Note);

                await _zoneRepository.AddAsync(zone);
                zoneList.Add(zone);
            }

            await _zoneRepository.SaveChangesAsync();

            return new BatchCreateZoneResult(zoneList.Select(x => x.Id).ToList());
        }
    }
}
