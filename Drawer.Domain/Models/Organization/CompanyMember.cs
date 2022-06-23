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
    public class CompanyMember : CompanyEntity<long>
    {
        public string UserId { get; private set; }

        public string UserEmail { get; private set; }

        public string UserName { get; private set; }

        public string Test { get; set; }

        public CompanyMember(string companyId, string userId, string userEmail, string userName)
        {
            CompanyId = companyId;
            UserId = userId;
            UserEmail = userEmail;
            UserName = userName;
        }
    }
}
