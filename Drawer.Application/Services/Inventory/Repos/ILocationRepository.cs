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
        Task<bool> ExistByName(string name);

        Task<bool> ExistByUpperLocationId(long locationId);

        Task<List<LocationQueryModel>> QueryAll();

        Task<LocationQueryModel?> QueryById(long id);
    }
}
