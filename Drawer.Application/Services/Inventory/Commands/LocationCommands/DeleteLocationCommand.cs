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

namespace Drawer.Application.Services.Inventory.Commands.LocationCommands
{
    public record DeleteLocationCommand(long Id) : ICommand;

    public class DeleteLocationCommandHandler : ICommandHandler<DeleteLocationCommand>
    {
        private readonly ILocationRepository _locationRepository;

        public DeleteLocationCommandHandler(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<Unit> Handle(DeleteLocationCommand command, CancellationToken cancellationToken)
        {
            // 위치가 참조된 곳이 있는지 확인한다.
            var location = await _locationRepository.FindByIdAsync(command.Id)
                ?? throw new EntityNotFoundException<Location>(command.Id);

            var followerExist = await _locationRepository.ExistByUpperLocationId(location.Id);
            if (followerExist)
                throw new AppException($"{location.Name}에 포함된 위치가 존재합니다");

            _locationRepository.Remove(location);
            await _locationRepository.SaveChangesAsync();
            return Unit.Value;

        }
    }
}
