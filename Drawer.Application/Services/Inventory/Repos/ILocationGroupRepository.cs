using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Domain.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.Repos
{
    public interface ILocationGroupRepository : IRepository<LocationGroup, long>
    {
        Task<bool> ExistByName(string name);

        Task<bool> ExistByParentGroup(long parentGroupId);

        Task<List<LocationGroupQueryModel>> QueryAll();

        Task<LocationGroupQueryModel?> QueryById(long id);
    }
}
