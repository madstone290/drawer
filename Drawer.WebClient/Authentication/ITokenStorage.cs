namespace Drawer.WebClient.Authentication
{
    public interface ITokenStorage
    {
        Task SaveAccessTokenAsync(string accessToken);

        Task<string?> GetAccessTokenAsync();

        Task ClearAccessTokenAsync();
    }
}
