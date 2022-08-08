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
        Task<IList<Item>> FindAll();

        Task<bool> ExistByName(string name);
    }
}
