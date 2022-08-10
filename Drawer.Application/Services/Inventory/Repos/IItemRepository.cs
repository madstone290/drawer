using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Domain.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.Repos
{
    public interface IItemRepository : IRepository<Item, long>
    {
        Task<bool> ExistByName(string name);

        Task<ItemQueryModel?> QueryById(long id);

        Task<List<ItemQueryModel>> QueryAll();
    }
}
