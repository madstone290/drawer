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
    /// 위치를 수정한다.
    /// </summary>
    public record UpdatePositionCommand(long Id, string Name) : ICommand<UpdatePositionResult>;

    public record UpdatePositionResult;

    public class UpdatePositionCommandHandler : ICommandHandler<UpdatePositionCommand, UpdatePositionResult>
    {
        private readonly IPositionRepository _positionRepository;

        public UpdatePositionCommandHandler(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public async Task<UpdatePositionResult> Handle(UpdatePositionCommand command, CancellationToken cancellationToken)
        {
            var position = await _positionRepository.FindByIdAsync(command.Id);
            if (position == null)
                throw new EntityNotFoundException<Position>(command.Id);

            position.SetName(command.Name);
            await _positionRepository.SaveChangesAsync();
            return new UpdatePositionResult();
        }
    }

}
