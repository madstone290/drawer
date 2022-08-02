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
    /// 장소를 수정한다.
    /// </summary>
    public record UpdateWorkPlaceCommand(long Id, string Name, string? Note) : ICommand<UpdateWorkPlaceResult>;

    public record UpdateWorkPlaceResult;

    public class UpdateWorkPlaceCommandHandler : ICommandHandler<UpdateWorkPlaceCommand, UpdateWorkPlaceResult>
    {
        private readonly IWorkplaceRepository _workPlaceRepository;

        public UpdateWorkPlaceCommandHandler(IWorkplaceRepository workPlaceRepository)
        {
            _workPlaceRepository = workPlaceRepository;
        }

        public async Task<UpdateWorkPlaceResult> Handle(UpdateWorkPlaceCommand command, CancellationToken cancellationToken)
        {
            var workPlace = await _workPlaceRepository.FindByIdAsync(command.Id);
            if (workPlace == null)
                throw new EntityNotFoundException<Workplace>(command.Id);

            workPlace.SetName(command.Name);
            workPlace.SetNote(command.Note);
            await _workPlaceRepository.SaveChangesAsync();

            return new UpdateWorkPlaceResult();
        }
    }

}
