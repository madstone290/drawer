using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Domain.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.Repos
{
    public interface IReceiptRepository : IRepository<Receipt, long>
    {
        Task<ReceiptQueryModel?> GetById(long id);

        Task<List<ReceiptQueryModel>>  GetByReceiptDateBetween(DateTime from, DateTime to);
    }
}
