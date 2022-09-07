namespace Drawer.Web.Pages.Organization.Models
{
    /// <summary>
    /// 가입 요청 내역
    /// </summary>
    public class JoinRequestModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime RequestTimeLocal { get; set; }
    }
}
