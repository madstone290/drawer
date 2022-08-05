using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.InventoryManagement.Repos;
using Drawer.Domain.Models.InventoryManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.InventoryManagement.Commands
{
    public record DeleteLocationCommand(long Id) : ICommand<DeleteLocationResult>;

    public record DeleteLocationResult;

    public class DeleteLocationCommandHandler : ICommandHandler<DeleteLocationCommand, DeleteLocationResult>
    {
        private readonly ILocationRepository _locationRepository;

        public DeleteLocationCommandHandler(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<DeleteLocationResult> Handle(DeleteLocationCommand command, CancellationToken cancellationToken)
        {
            // 위치가 참조된 곳이 있는지 확인한다.
            var location = await _locationRepository.FindByIdAsync(command.Id)
                ?? throw new EntityNotFoundException<Location>(command.Id);

            var followerExist = await _locationRepository.ExistByUpperLocationId(location.Id);
            if (followerExist)
                throw new AppException($"{location.Name}에 포함된 위치가 존재합니다");

            _locationRepository.Remove(location);
            await _locationRepository.SaveChangesAsync();
            return new DeleteLocationResult();

        }
    }
}
