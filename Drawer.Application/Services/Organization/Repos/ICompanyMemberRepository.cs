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
        Task<bool> ExistByUserId(long userId);

        Task<CompanyMember?> FindByUserIdAsync(long userId);

        Task<List<CompanyMemberQueryModel>> QueryByCompanyId(long companyId);

    }
}
