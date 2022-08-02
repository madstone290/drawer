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
    public record BatchCreateWorkplaceCommand(IList<BatchCreateWorkplaceCommand.Workplace> WorkplaceList)
        : ICommand<BatchCreateWorkplaceResult>
    {
        public record Workplace(string Name, string? Note);
    }

    public record BatchCreateWorkplaceResult(IList<long> IdList);

    public class BatchCreateWorkplaceCommandHandler : ICommandHandler<BatchCreateWorkplaceCommand, BatchCreateWorkplaceResult>
    {
        private readonly IWorkplaceRepository _workplaceRepository;

        public BatchCreateWorkplaceCommandHandler(IWorkplaceRepository workplaceRepository)
        {
            _workplaceRepository = workplaceRepository;
        }

        public async Task<BatchCreateWorkplaceResult> Handle(BatchCreateWorkplaceCommand command, CancellationToken cancellationToken)
        {
            var workplaceList = new List<Workplace>();
            foreach (var workplaceDto in command.WorkplaceList)
            {
                var workplace = new Workplace(workplaceDto.Name);
                workplace.SetNote(workplaceDto.Note);

                await _workplaceRepository.AddAsync(workplace);
                workplaceList.Add(workplace);
            }

            await _workplaceRepository.SaveChangesAsync();

            return new BatchCreateWorkplaceResult(workplaceList.Select(x => x.Id).ToList());
        }
    }
}
