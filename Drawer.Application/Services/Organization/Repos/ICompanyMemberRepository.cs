using Drawer.Application.Services.Organization.QueryModels;
using Drawer.Domain.Models.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Organization.Repos
{
    public interface ICompanyMemberRepository : IRepository<CompanyMember>
    {
        Task<CompanyMember?> FindByUserIdAsync(string userId);

        Task<List<CompanyMemberQueryModel>> QueryByCompanyId(string companyId);

    }
}
