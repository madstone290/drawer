using Drawer.Domain.Models.InventoryManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.InventoryManagement.Repos
{
    public interface IInventoryDetailRepository : IRepository<InventoryDetail>
    {
        Task<InventoryDetail?> FindByItemIdAndLocationIdAsync(long itemId, long locationId);

        Task<IList<InventoryDetail>> FindByItemIdAsync(long itemId);

        Task<IList<InventoryDetail>> FindByLocationIdAsync(long locationId);

        Task<IList<InventoryDetail>> FindAll();
     
    }
}
