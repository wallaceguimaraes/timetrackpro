using System.Security.Cryptography;
using System.Text;

namespace api.Infrastructure.Security
{
    public class UrlEncryption
    {
        private static byte[] _bIV =
        {
            0x50, 0x08, 0xF1, 0xDD, 0xDE, 0x3C, 0xF2, 0x18,
            0x44, 0x74, 0x19, 0x2C, 0x53, 0x49, 0xAB, 0xBC
        };

        private const string CryptoKey = "Q3JpcHRvZ3JhZmlhcyBjb20gUmluamRhZWwgLyBBRVM=";

        public string Decrypt(string encryptedText)
        {
            byte[] bKey = Convert.FromBase64String(CryptoKey);
            byte[] bText = Convert.FromBase64String(encryptedText);

            Rijndael rijndael = new RijndaelManaged();
            rijndael.KeySize = 256;

            MemoryStream stream = new MemoryStream();
            CryptoStream decryptor = new CryptoStream(stream, rijndael.CreateDecryptor(bKey, _bIV), CryptoStreamMode.Write);

            decryptor.Write(bText, 0, bText.Length);
            decryptor.FlushFinalBlock();

            return new UTF8Encoding().GetString(stream.ToArray());
        }

        public string Encrypt(string plainText)
        {
            byte[] bKey = Convert.FromBase64String(CryptoKey);
            byte[] bText = new UTF8Encoding().GetBytes(plainText);

            Rijndael rijndael = new RijndaelManaged();
            rijndael.KeySize = 256;

            MemoryStream stream = new MemoryStream();
            CryptoStream encryptor = new CryptoStream(stream, rijndael.CreateEncryptor(bKey, _bIV), CryptoStreamMode.Write);
            encryptor.Write(bText, 0, bText.Length);
            encryptor.FlushFinalBlock();

            return Convert.ToBase64String(stream.ToArray());
        }
    }
}