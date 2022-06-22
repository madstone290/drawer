namespace Drawer.WebClient.Authentication
{
    public interface ITokenManager
    {
        /// <summary>
        /// 액세스 토큰을 가져온다.
        /// </summary>
        /// <returns></returns>
        Task<TokenResult> GetAccessTokenAsync();

        /// <summary>
        /// 액세스 토큰을 갱신한다
        /// </summary>
        /// <returns></returns>
        Task<TokenResult> RefreshAccessTokenAsync();

    }
}
