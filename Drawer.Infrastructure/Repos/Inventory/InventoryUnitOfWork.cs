using Drawer.Application.Services.Inventory.Repos;
using Drawer.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Infrastructure.Repos.Inventory
{
    public class InventoryUnitOfWork : IInventoryUnitOfWork
    {
        private readonly DrawerDbContext _dbContext;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IIssueRepository _issueRepository;
        private readonly IInventoryItemRepository _inventoryItemRepository;


        public IReceiptRepository ReceiptRepository => _receiptRepository;
        public IIssueRepository IssueRepository => _issueRepository;
        public IInventoryItemRepository InventoryItemRepository => _inventoryItemRepository;

        public InventoryUnitOfWork(DrawerDbContext dbContext,
                                   IReceiptRepository receiptRepository,
                                   IIssueRepository issueRepository,
                                   IInventoryItemRepository inventoryItemRepository)
        {
            _dbContext = dbContext;
            _receiptRepository = receiptRepository;
            _issueRepository = issueRepository;
            _inventoryItemRepository = inventoryItemRepository;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
