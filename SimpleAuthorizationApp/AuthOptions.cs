using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SimpleAuthorizationApp
{
    internal class AuthOptions
    {
        internal const string ISSUER = "SimpleAuthServer";
        internal const string AUDIENCE = "SimpleAuthClient";
        const string KEY = "supersecret_key!123";
        internal static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));

    }
}
