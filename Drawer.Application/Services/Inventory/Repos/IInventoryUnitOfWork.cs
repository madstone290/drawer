using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.Repos
{
    public interface IInventoryUnitOfWork : IUnitOfWork
    {
        IReceiptRepository ReceiptRepository { get; }
        IIssueRepository IssueRepository { get; }
        IInventoryItemRepository InventoryItemRepository { get; }
    }
}
