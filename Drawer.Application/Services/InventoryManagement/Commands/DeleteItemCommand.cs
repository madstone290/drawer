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
    /// <summary>
    /// 아이템을 삭제한다.
    /// 다른 엔티티에서 아이템에 대한 참조가 없고 재고수량이 0인 경우에만 삭제가 가능하다.
    /// </summary>
    public record DeleteItemCommand(long Id) : ICommand<DeleteItemResult>;

    public record DeleteItemResult;

    public class DeleteItemCommandHandler : ICommandHandler<DeleteItemCommand, DeleteItemResult>
    {
        private readonly IItemRepository _itemRepository;

        public DeleteItemCommandHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<DeleteItemResult> Handle(DeleteItemCommand command, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.FindByIdAsync(command.Id)
                ?? throw new EntityNotFoundException<Item>(command.Id);
            // 재고수량 확인
            
            // 참조 확인

            _itemRepository.Remove(item);
            await _itemRepository.SaveChangesAsync();
            return new DeleteItemResult();

        }
    }
}
