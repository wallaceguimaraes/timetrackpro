using System;
using System.Security.Cryptography;
using System.Text;

namespace api.Infrastructure.Security
{
    public class Sha256Hash
    {
        private string _plainText;
        private string _salt;

        public Sha256Hash(string plainText)
            : this(plainText, string.Empty) { }

        public Sha256Hash(string plainText, string salt)
        {
            _plainText = plainText;
            _salt = salt;
        }

        public byte[] ToArray()
        {
            byte[] bytes = Encoding.UTF8.GetBytes(_plainText + _salt);

            using (var sha = SHA256.Create())
            {
                return sha.ComputeHash(bytes);
            }
        }

        public string ToBase64String()
        {
            return Convert.ToBase64String(ToArray());
        }

        public string ToHexadecimalString()
        {
            return BitConverter.ToString(ToArray())
                .Replace("-", string.Empty).ToLower();
        }
    }
}