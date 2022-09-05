using Drawer.Web.Pages.Organization.Models;

namespace Drawer.Web.Pages.Organization
{
    public partial class CompanyMemberRequest
    {
        private readonly List<CompanyMemberModel> _companyMemberList = new();

        public int MemberCount => _companyMemberList.Count;

        void Load_Click()
        {

        }

        void Accept_Click()
        {

        }
    }
}
