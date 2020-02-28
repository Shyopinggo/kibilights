using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KibiLights
{
    public static class Tools
    {
        public static string HashPassword(string input)
        {
            if (string.IsNullOrEmpty(input)) return null;
            var bytes = Encoding.UTF8.GetBytes(input);
            using (var hasher = SHA512.Create())
            {
                var resultBytes = hasher.ComputeHash(bytes);
                var result = new StringBuilder();
                foreach (var b in resultBytes)
                    result.Append(b.ToString("X2"));
                return result.ToString();
            }
        }
    }
}
