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
    /// 위치를 삭제한다.
    /// </summary>
    public record DeletePositionCommand(long Id) : ICommand<DeletePositionResult>;

    public record DeletePositionResult;

    public class DeletePositionCommandHandler : ICommandHandler<DeletePositionCommand, DeletePositionResult>
    {
        private readonly IPositionRepository _positionRepository;

        public DeletePositionCommandHandler(IPositionRepository positionRepository)
        {
            _positionRepository =positionRepository;
        }

        public async Task<DeletePositionResult> Handle(DeletePositionCommand command, CancellationToken cancellationToken)
        {
            var position = await _positionRepository.FindByIdAsync(command.Id);
            if (position == null)
                throw new EntityNotFoundException<Position>(command.Id);
            
            _positionRepository.Remove(position);
            await _positionRepository.SaveChangesAsync();
            return new DeletePositionResult();

        }
    }
}
