using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.Repos;
using Drawer.Domain.Models.Inventory;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.Commands
{
    public record LocationGroupUpdateCommand(long Id, LocationGroupUpdateCommandModel LocationGroup) : ICommand;

    public class LocationGroupUpdateCommandHandler : ICommandHandler<LocationGroupUpdateCommand>
    {
        private readonly ILocationGroupRepository _groupRepository;

        public LocationGroupUpdateCommandHandler(ILocationGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<Unit> Handle(LocationGroupUpdateCommand command, CancellationToken cancellationToken)
        {
            var groupId = command.Id;
            var groupDto = command.LocationGroup;

            var group = await _groupRepository.FindByIdAsync(groupId)
                ?? throw new EntityNotFoundException<LocationGroup>(groupId);

            
            if(!EqualityComparer<string>.Default.Equals(groupDto.Name, group.Name))
            {
                if (await _groupRepository.ExistByName(groupDto.Name))
                    throw new AppException($"동일한 그룹명이 존재합니다. {groupDto.Name}");
                group.SetName(groupDto.Name);
            }
                
            group.SetNote(groupDto.Note);

            await _groupRepository.SaveChangesAsync();
            return Unit.Value;
        }
    }

}
