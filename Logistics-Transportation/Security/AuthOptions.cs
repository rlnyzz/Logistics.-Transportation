using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Logistics_Transportation.Security
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer";
        public const string AUDIENCE = "Logistics-Transportation";
        const string KEY = "supersecretkey_logistic123949494934280425";
        public static SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
