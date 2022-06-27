using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models.Organization
{
    /// <summary>
    /// 회사 구성원
    /// </summary>
    public class CompanyMember : AuditableEntity<long>
    {
        public string CompanyId { get; private set; }

        public string UserId { get; private set; }

        public CompanyMember(string companyId, string userId)
        {
            CompanyId = companyId;
            UserId = userId;
        }
    }
}
