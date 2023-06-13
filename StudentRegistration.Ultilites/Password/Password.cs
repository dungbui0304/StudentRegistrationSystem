using System.Security.Cryptography;
using System.Text;

namespace StudentRegistration.Ultilites.HashPassword
{
    public static class Password
    {
        public static string HashPassword(string password)
        {
            var bytes = new UTF8Encoding().GetBytes(password);
            var hashBytes = SHA256.Create().ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }

        public static string GenerateDefaultPassword()
        {
            const int passwordLength = 8;
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
            var randNum = new Random();
            var chars = new char[passwordLength];

            for (var i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[(int)(allowedChars.Length * randNum.NextDouble())];
            }

            return new string(chars);
        }
    }
}
