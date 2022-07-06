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
    /// 자리를 수정한다.
    /// </summary>
    public record UpdateSpotCommand(long Id, string Name, string? Note) : ICommand<UpdateSpotResult>;

    public record UpdateSpotResult;

    public class UpdateSpotCommandHandler : ICommandHandler<UpdateSpotCommand, UpdateSpotResult>
    {
        private readonly ISpotRepository _spotRepository;

        public UpdateSpotCommandHandler(ISpotRepository spotRepository)
        {
            _spotRepository = spotRepository;
        }

        public async Task<UpdateSpotResult> Handle(UpdateSpotCommand command, CancellationToken cancellationToken)
        {
            var position = await _spotRepository.FindByIdAsync(command.Id);
            if (position == null)
                throw new EntityNotFoundException<Spot>(command.Id);

            position.SetName(command.Name);
            position.SetNote(command.Note);

            await _spotRepository.SaveChangesAsync();
            return new UpdateSpotResult();
        }
    }

}
