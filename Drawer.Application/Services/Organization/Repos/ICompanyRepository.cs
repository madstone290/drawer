using Drawer.Application.Services.Organization.QueryModels;
using Drawer.Domain.Models.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Organization.Repos
{
    public interface ICompanyRepository : IRepository<Company, long>
    {
        Task<bool> ExistByOwnerId(long ownerId);
        
        /// <summary>
        /// 회사 소유자의 이메일로 회사를 조회한다
        /// </summary>
        /// <param name="companyOwnerEmail">소유자 이메일</param>
        /// <returns></returns>
        Task<Company?> FindByOwnerEmail(string companyOwnerEmail);

        Task<CompanyQueryModel?> QueryById(long id);
        
    }
}
