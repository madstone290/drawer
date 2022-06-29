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
    /// 작업장을 수정한다.
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Name"></param>
    /// <param name="Description"></param>
    public record UpdateWorkPlaceCommand(long Id, string Name, string? Description) : ICommand<UpdateWorkPlaceResult>;

    public record UpdateWorkPlaceResult;

    public class UpdateWorkPlaceCommandHandler : ICommandHandler<UpdateWorkPlaceCommand, UpdateWorkPlaceResult>
    {
        private readonly IWorkPlaceRepository _workPlaceRepository;

        public UpdateWorkPlaceCommandHandler(IWorkPlaceRepository workPlaceRepository)
        {
            _workPlaceRepository = workPlaceRepository;
        }

        public async Task<UpdateWorkPlaceResult> Handle(UpdateWorkPlaceCommand command, CancellationToken cancellationToken)
        {
            var workPlace = await _workPlaceRepository.FindByIdAsync(command.Id);
            if (workPlace == null)
                throw new EntityNotFoundException<WorkPlace>(command.Id);

            workPlace.SetName(command.Name);
            workPlace.SetDescription(command.Description);
            await _workPlaceRepository.SaveChangesAsync();

            return new UpdateWorkPlaceResult();
        }
    }

}
