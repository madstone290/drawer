using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Domain.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.Repos
{
    public interface ILayoutRepository : IRepository<Layout, long>
    {
        Task<LayoutQueryModel?> QueryById(long id);

        Task<List<LayoutQueryModel>> QueryAll();
    }
}
