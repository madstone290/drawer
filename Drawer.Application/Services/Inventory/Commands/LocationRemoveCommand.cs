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
    public record LocationRemoveCommand(long Id) : ICommand;

    public class LocationRemoveCommandHandler : ICommandHandler<LocationRemoveCommand>
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LocationRemoveCommandHandler(ILocationRepository locationRepository, IUnitOfWork unitOfWork)
        {
            _locationRepository = locationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(LocationRemoveCommand command, CancellationToken cancellationToken)
        {
            // 위치가 참조된 곳이 있는지 확인한다.
            var location = await _locationRepository.FindByIdAsync(command.Id)
                ?? throw new EntityNotFoundException<Location>(command.Id);

            _locationRepository.Remove(location);
            await _unitOfWork.CommitAsync();
            return Unit.Value;

        }
    }
}
