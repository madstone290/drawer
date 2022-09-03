using Drawer.Application.Config;
using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.Repos;
using Drawer.Domain.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.Commands
{
    public record LocationGroupBatchAddCommand(List<LocationGroupAddCommandModel> LocationGroupList) : ICommand<List<long>>;

    public class LocationGroupBatchAddCommandHandler : ICommandHandler<LocationGroupBatchAddCommand, List<long>>
    {
        private readonly ILocationGroupRepository _groupRepository;

        public LocationGroupBatchAddCommandHandler(ILocationGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<List<long>> Handle(LocationGroupBatchAddCommand command, CancellationToken cancellationToken)
        {
            var groupList = new List<LocationGroup>();
            foreach (var groupDto in command.LocationGroupList)
            {
                var parentGroup = groupDto.ParentGroupId.HasValue
                    ? await _groupRepository.FindByIdAsync(groupDto.ParentGroupId.Value)
                    : null;

                if (await _groupRepository.ExistByName(groupDto.Name))
                    throw new AppException($"동일한 그룹명이 존재합니다. {groupDto.Name}");

                var group = new LocationGroup(groupDto.Name, parentGroup);
                group.SetNote(groupDto.Note);

                await _groupRepository.AddAsync(group);
                groupList.Add(group);
            }

            await _groupRepository.SaveChangesAsync();

            return groupList.Select(x => x.Id).ToList();
        }
    }
}
