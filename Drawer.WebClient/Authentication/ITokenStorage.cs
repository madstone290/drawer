namespace Drawer.WebClient.Authentication
{
    /// <summary>
    /// 토큰 저장소. 토큰을 보관한다.
    /// </summary>
    public interface ITokenStorage
    {
        Task SaveAccessTokenAsync(string accessToken);

        Task<string?> GetAccessTokenAsync();

        Task ClearAccessTokenAsync();
    }
}
