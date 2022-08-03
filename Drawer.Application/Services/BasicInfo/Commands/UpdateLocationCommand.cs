using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.BasicInfo.Repos;
using Drawer.Domain.Models.BasicInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.BasicInfo.Commands
{
    public record UpdateLocationCommand(long Id, string Name, string? Note) : ICommand<UpdateLocationResult>;

    public record UpdateLocationResult;

    public class UpdateLocationCommandHandler : ICommandHandler<UpdateLocationCommand, UpdateLocationResult>
    {
        private readonly ILocationRepository _locationRepository;

        public UpdateLocationCommandHandler(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<UpdateLocationResult> Handle(UpdateLocationCommand command, CancellationToken cancellationToken)
        {
            var location = await _locationRepository.FindByIdAsync(command.Id)
                ?? throw new EntityNotFoundException<Location>(command.Id);

            location.SetName(command.Name);
            location.SetNote(command.Note);

            await _locationRepository.SaveChangesAsync();
            return new UpdateLocationResult();
        }
    }

}
