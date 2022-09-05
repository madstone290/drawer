using Drawer.Domain.Models.UserInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models.Organization
{
    /// <summary>
    /// 회사 구성원.
    /// 사용자는 하나의 회사에만 속할 수 있다.
    /// </summary>
    public class CompanyMember : AuditableEntity<long>
    {
        public Company Company { get; private set; } = null!;
        public long CompanyId { get; private set; }

        public User User { get; private set; } = null!;
        public long UserId { get; private set; }

        public bool IsOwner { get; private set; }


        private CompanyMember() { }
        public CompanyMember(Company company, User user, bool isOwner)
        {
            Company = company;
            CompanyId = company.Id;
            User = user;
            UserId = user.Id;
            IsOwner = isOwner;
        }
    }
}
