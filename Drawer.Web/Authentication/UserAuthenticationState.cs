namespace Drawer.Web.Authentication
{
    /// <summary>
    /// 현재 사용자 인증 상태
    /// </summary>
    public class UserAuthenticationState
    {
        /// <summary>
        /// 로그인 여부
        /// </summary>
        public bool IsAuthenticated { get; }

        /// <summary>
        /// 이메일
        /// </summary>
        public string? Email { get; }

        /// <summary>
        /// 리프레시 토큰
        /// </summary>
        public string? RefreshToken { get; }

        private UserAuthenticationState(bool isLoggedIn, string? email, string? refreshToken)
        {
            IsAuthenticated = isLoggedIn;
            Email = email;
            RefreshToken = refreshToken;
        }
        public static UserAuthenticationState Unauthenticated() => new(false, null, null);
        public static UserAuthenticationState Authenticated(string email, string refreshToken) => new(true, email, refreshToken);
    }
}
