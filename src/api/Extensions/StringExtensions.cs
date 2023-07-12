using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using api.Infrastructure.Security;

namespace api.Extensions
{
    public static class StringExtensions
    {
        public static string Encrypt(this string str, string salt)
        {
            if (string.IsNullOrWhiteSpace(str)) return str;
            return new Sha256Hash(str, salt).ToBase64String();
        }

        public static string FirstWord(this string str, char separator)
        {
            if (string.IsNullOrWhiteSpace(str)) return str;
            return str.Split(separator).FirstOrDefault();
        }

        public static string Truncate(this string str, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(str)) return str;
            return str.Length <= maxLength ? str : str.Substring(0, maxLength);
        }

        public static string NoAccents(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;

            str = WebUtility.HtmlDecode(str);
            str = str.Normalize(NormalizationForm.FormD);

            char[] chars = str.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();

            return new string(chars).Normalize(NormalizationForm.FormC);
        }

        public static string OnlyNumbers(this string str)
        {
            if (str == null) return str;
            return Regex.Replace(str, "[^\\d]+", string.Empty);
        }

        public static string OnlyLetters(this string str)
        {
            if (str == null) return str;
            return Regex.Replace(str, "[^A-Za-z]+", string.Empty);
        }


        public static string[] Words(this string str)
        {
            if (str == null) return new string[] { };
            return str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static bool In(this string str, params string[] values)
        {
            if (string.IsNullOrWhiteSpace(str))
                return false;

            return values.Any(value => string.Equals(str, value, StringComparison.OrdinalIgnoreCase));
        }
    }
}