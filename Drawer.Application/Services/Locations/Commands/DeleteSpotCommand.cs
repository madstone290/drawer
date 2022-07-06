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
    /// 자리를 삭제한다.
    /// </summary>
    public record DeleteSpotCommand(long Id) : ICommand<DeletePositionResult>;

    public record DeletePositionResult;

    public class DeletePositionCommandHandler : ICommandHandler<DeleteSpotCommand, DeletePositionResult>
    {
        private readonly ISpotRepository _spotRepository;

        public DeletePositionCommandHandler(ISpotRepository positionRepository)
        {
            _spotRepository =positionRepository;
        }

        public async Task<DeletePositionResult> Handle(DeleteSpotCommand command, CancellationToken cancellationToken)
        {
            var position = await _spotRepository.FindByIdAsync(command.Id);
            if (position == null)
                throw new EntityNotFoundException<Spot>(command.Id);
            
            _spotRepository.Remove(position);
            await _spotRepository.SaveChangesAsync();
            return new DeletePositionResult();

        }
    }
}
