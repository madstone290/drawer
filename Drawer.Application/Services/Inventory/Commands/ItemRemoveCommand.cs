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
    /// <summary>
    /// 아이템을 삭제한다.
    /// 다른 엔티티에서 아이템에 대한 참조가 없고 재고수량이 0인 경우에만 삭제가 가능하다.
    /// </summary>
    public record ItemRemoveCommand(long Id) : ICommand;

    public class ItemRemoveCommandHandler : ICommandHandler<ItemRemoveCommand>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ItemRemoveCommandHandler(IItemRepository itemRepository, IUnitOfWork unitOfWork)
        {
            _itemRepository = itemRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(ItemRemoveCommand command, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.FindByIdAsync(command.Id)
                ?? throw new EntityNotFoundException<Item>(command.Id);
            // 재고수량 확인

            // 참조 확인

            _itemRepository.Remove(item);
            await _unitOfWork.CommitAsync();
            return Unit.Value;

        }
    }
}
