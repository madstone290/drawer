using Drawer.Domain.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Items.Repos
{
    public interface IItemRepository : IRepository<Item, long>
    {
        Task<IList<Item>> FindAll();
    }
}
