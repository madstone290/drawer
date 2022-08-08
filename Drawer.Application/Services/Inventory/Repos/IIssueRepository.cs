using Drawer.Domain.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.Repos
{
    public interface IIssueRepository : IRepository<Issue, long>
    {
        Task<IList<Issue>> FindByIssueDateBetween(DateTime from, DateTime to);
    }
}
