using Drawer.Application.Config;
using Drawer.Application.Services.Inventory.Repos;
using Drawer.Domain.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.Commands
{
    public record CreateLocationCommand(long? ParentGroupId, string Name, string? Note, bool IsGroup) : ICommand<CreateLocationResult>;

    public record CreateLocationResult(long Id);

    public class CreateLocationCommandHandler : ICommandHandler<CreateLocationCommand, CreateLocationResult>
    {
        private readonly ILocationRepository _locationRepository;

        public CreateLocationCommandHandler(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<CreateLocationResult> Handle(CreateLocationCommand command, CancellationToken cancellationToken)
        {
            if (await _locationRepository.ExistByName(command.Name))
                throw new AppException($"동일한 이름이 존재합니다. {command.Name}");

            var parentGroup = command.ParentGroupId.HasValue
                ? await _locationRepository.FindByIdAsync(command.ParentGroupId.Value)
                : null;
            var location = new Location(parentGroup, command.Name, command.IsGroup);
            location.SetNote(command.Note);

            await _locationRepository.AddAsync(location);
            await _locationRepository.SaveChangesAsync();

            return new CreateLocationResult(location.Id);
        }
    }
}
