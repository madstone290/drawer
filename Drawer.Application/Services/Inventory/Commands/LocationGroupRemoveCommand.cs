using Drawer.Application.Config;
using Drawer.Application.Exceptions;
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
    public record LocationGroupRemoveCommand(long Id) : ICommand;

    public class LocationGroupRemoveCommandHandler : ICommandHandler<LocationGroupRemoveCommand>
    {
        private readonly ILocationGroupRepository _groupRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LocationGroupRemoveCommandHandler(ILocationGroupRepository groupRepository, ILocationRepository locationRepository, IUnitOfWork unitOfWork)
        {
            _groupRepository = groupRepository;
            _locationRepository = locationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(LocationGroupRemoveCommand command, CancellationToken cancellationToken)
        {
            // 위치가 참조된 곳이 있는지 확인한다.
            var group = await _groupRepository.FindByIdAsync(command.Id)
                ?? throw new EntityNotFoundException<LocationGroup>(command.Id);

            var childGroupExist = await _groupRepository.ExistByParentGroup(group.Id);
            if (childGroupExist)
                throw new AppException($"{group.Name}에 포함된 그룹이 존재합니다");

            var childLocationExist = await _locationRepository.ExistByGroup(group.Id);
            if(childLocationExist)
                throw new AppException($"{group.Name}에 포함된 위치가 존재합니다");

            _groupRepository.Remove(group);

            await _unitOfWork.CommitAsync();
            return Unit.Value;

        }
    }
}
