namespace Drawer.Web.Pages.Organization.Models
{
    public class CompanyMemberModel
    {
        public long UserId { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
