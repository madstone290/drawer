namespace Drawer.Web.Encryption
{
    public interface IEncryptionService
    {
        string Encrypt(string plainText);

        string? Decrypt(string cipherText);
    }
}
