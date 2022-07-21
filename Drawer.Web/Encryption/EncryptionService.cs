using System.Security.Cryptography;
using System.Text;

namespace Drawer.Web.Encryption
{
    public class EncryptionService : IEncryptionService
    {
        static string aes_key = "AXe8YwuIn1zxt3FPWTZFlAa14EHdPAdN9FaZ9RQWihc=";
        static string aes_iv = "bsxnWolsAyO7kCfWuyrnqg==";

        private readonly Aes aes;

        public EncryptionService()
        {
            aes = Aes.Create();
            aes.Key = Convert.FromBase64String(aes_key);
            aes.IV = Convert.FromBase64String(aes_iv);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
        }

        public string Encrypt(string plainText)
        {
            ICryptoTransform encryptor = aes.CreateEncryptor();

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (var streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }
                }
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        public string? Decrypt(string cipherText)
        {
            ICryptoTransform decryptor = aes.CreateDecryptor();

            try
            {
                byte[] buffer = Convert.FromBase64String(cipherText);
                using (var memoryStream = new MemoryStream(buffer))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
