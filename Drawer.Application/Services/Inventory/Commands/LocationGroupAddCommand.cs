using Drawer.Application.Config;
using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.Repos;
using Drawer.Domain.Models.Inventory;

namespace Drawer.Application.Services.Inventory.Commands
{
    public record LocationGroupAddCommand(LocationGroupAddCommandModel LocationGroup) : ICommand<long>;

    public class LocationGroupAddCommandHandler : ICommandHandler<LocationGroupAddCommand, long>
    {
        private readonly ILocationGroupRepository _groupRepository;

        public LocationGroupAddCommandHandler(ILocationGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<long> Handle(LocationGroupAddCommand command, CancellationToken cancellationToken)
        {
            var groupDto = command.LocationGroup;

            if (await _groupRepository.ExistByName(groupDto.Name))
                throw new AppException($"동일한 그룹명이 존재합니다. {groupDto.Name}");

            var parentGroup = groupDto.ParentGroupId.HasValue
                ? await _groupRepository.FindByIdAsync(groupDto.ParentGroupId.Value)
                : null;
            var group = new LocationGroup(groupDto.Name, parentGroup);
            group.SetNote(groupDto.Note);

            await _groupRepository.AddAsync(group);
            await _groupRepository.SaveChangesAsync();

            return group.Id;
        }
    }
}
