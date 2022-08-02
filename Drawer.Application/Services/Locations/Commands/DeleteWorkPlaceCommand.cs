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
    /// 장소를 삭제한다.
    /// </summary>
    /// <param name="Id">장소Id</param>
    public record DeleteWorkPlaceCommand(long Id) : ICommand<DeleteWorkPlaceResult>;

    public record DeleteWorkPlaceResult;

    public class DeleteWorkPlaceCommandHandler : ICommandHandler<DeleteWorkPlaceCommand, DeleteWorkPlaceResult>
    {
        private readonly IWorkplaceRepository _workPlaceRepository;

        public DeleteWorkPlaceCommandHandler(IWorkplaceRepository workPlaceRepository)
        {
            _workPlaceRepository = workPlaceRepository;
        }

        public async Task<DeleteWorkPlaceResult> Handle(DeleteWorkPlaceCommand command, CancellationToken cancellationToken)
        {
            var workPlace = await _workPlaceRepository.FindByIdAsync(command.Id);
            if (workPlace == null)
                throw new EntityNotFoundException<Workplace>(command.Id);
            
            _workPlaceRepository.Remove(workPlace);
            await _workPlaceRepository.SaveChangesAsync();
            return new DeleteWorkPlaceResult();

        }
    }
}
