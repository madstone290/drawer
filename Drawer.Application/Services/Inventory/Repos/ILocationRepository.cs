using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Domain.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.Repos
{
    public interface ILocationRepository : IRepository<Location, long>
    {
        Task<bool> ExistByGroup(long groupId);

        Task<bool> ExistByName(string name);

        Task<List<LocationQueryModel>> QueryAll();

        Task<LocationQueryModel?> QueryById(long id);
    }
}
