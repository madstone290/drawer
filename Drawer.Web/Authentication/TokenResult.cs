namespace Drawer.Web.Authentication
{
    public class TokenResult
    {
        public bool IsSuccessful { get; }

        public string? AccessToken { get; }

        public TokenResult(bool isSuccessful, string? accessToken)
        {
            IsSuccessful = isSuccessful;
            AccessToken = accessToken;
        }
    }
}
