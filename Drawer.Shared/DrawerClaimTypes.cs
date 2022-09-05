namespace Drawer.Shared
{
    /// <summary>
    /// Drawer프로젝트에서 사용하는 클레임 유형
    /// </summary>
    public static class DrawerClaimTypes
    {
        public const string AccessToken = "AccessToken";
        public const string RefreshToken = "RefreshToken";
        public const string CompanyId = "CompanyId";
        public const string UserId = "UserId";

        public const string IdentityUserId = "IdentityUserId";
        public const string IsCompanyOwner = "IsCompanyOwner";
        public const string IsCompanyMember = "IsCompanyMember";
    }
}