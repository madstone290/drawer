using Drawer.Application.Services.Organization.QueryModels;
using Drawer.Domain.Models.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Organization.Repos
{
    public interface ICompanyJoinRequestRepository : IRepository<CompanyJoinRequest, long>
    {
        /// <summary>
        /// 처리되지 않은 요청이 존재하는지 조회한다
        /// </summary>
        /// <param name="companyId">회사ID</param>
        /// <param name="userId">사용자ID</param>
        /// <returns></returns>
        Task<bool> ExistUnhandledRequestByCompanyIdAndUserId(long companyId, long userId);

        /// <summary>
        /// 처리되지 않은 요청을 조회한다.
        /// </summary>
        /// <param name="userId">가입을 요청한 사용자의 ID</param>
        /// <returns></returns>
        Task<List<CompanyJoinRequest>> FindUnhandledRequestByUserId(long userId);

        /// <summary>
        /// 회사 가입요청을 조회한다
        /// </summary>
        /// <param name="companyId">조회할 회사ID</param>
        /// <returns></returns>
        Task<List<CompanyJoinRequestQueryModel>> QueryByCompanyId(long companyId);
    }
}
