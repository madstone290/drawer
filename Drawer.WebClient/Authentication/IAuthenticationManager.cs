namespace Drawer.WebClient.Authentication
{
    /// <summary>
    /// 인증 기능을 제공한다.
    /// </summary>
    public interface IAuthenticationManager
    {
        /// <summary>
        /// 로그인을 진행한다.
        /// </summary>
        /// <returns></returns>
        Task<AuthenticationResult> LoginAsync(string email, string password);
        
        /// <summary>
        /// 로그아웃을 진행한다.
        /// </summary>
        /// <returns></returns>
        Task<AuthenticationResult> LogoutAsync();

        /// <summary>
        /// 유저의 상태를 확인한다.
        /// </summary>
        /// <returns></returns>
        Task<UserAuthenticationState> GetUserStateAsync();

        /// <summary>
        /// 권한을 확인한다.
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        Task<bool> AuthorizeAsync(string permission);
        
    }
}
