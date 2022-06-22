namespace Drawer.WebClient.Authentication
{
    public record AuthenticationResult(bool IsSuccessful, bool IsUnconfirmedEmail, string ErrorMessage)
    {
        public static AuthenticationResult Success() 
            => new(true, false, string.Empty);

        public static AuthenticationResult Fail(string message) 
            => new(false, false, message);

        public static AuthenticationResult UnconfirmedEmail() 
            => new(false, true, string.Empty);
    }
}
