using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Drawer.Contract.Organization.GetCompanyMembersResponse;

namespace Drawer.Contract.Organization
{
    public record GetCompanyMembersResponse(IList<Member> Members)
    {
        public record Member(string CompanyId, string UserId);
    }
}
