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
    /// <summary>
    /// 장소를 생성한다.
    /// </summary>
    public record CreateWorkPlaceCommand(string Name, string? Note) : ICommand<CreateWorkPlaceResult>;

    public record CreateWorkPlaceResult(long Id);

    public class CreateWorkPlaceCommandHandler : ICommandHandler<CreateWorkPlaceCommand, CreateWorkPlaceResult>
    {
        private readonly IWorkPlaceRepository _workPlaceRepository;

        public CreateWorkPlaceCommandHandler(IWorkPlaceRepository workPlaceRepository)
        {
            _workPlaceRepository = workPlaceRepository;
        }

        public async Task<CreateWorkPlaceResult> Handle(CreateWorkPlaceCommand command, CancellationToken cancellationToken)
        {
            var workPlace = new WorkPlace(command.Name);
            workPlace.SetNote(command.Note);
            await _workPlaceRepository.AddAsync(workPlace);
            await _workPlaceRepository.SaveChangesAsync();

            return new CreateWorkPlaceResult(workPlace.Id);
        }
    }
}
