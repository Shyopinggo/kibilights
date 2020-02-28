using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace KibiLights
{
    public class AuthOptions
    {
        public const string ISSUER = "Kibilights.com";
        public const string AUDIENCE = "KibilightsClient";
        private const string KEY = "kibilights.comVv179280129";
        public const int LIFETIME = 1;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
