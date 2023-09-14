using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Auth
{
    public class JwtClaimTypes
    {
        public const string Subject = "sub";
        public const string Name = "name";
        public const string GivenName = "given_name";
        public const string FamilyName = "family_name";
        public const string MiddleName = "middle_name";
        public const string NickName = "nickname";
        public const string Profile = "profile";
        public const string Email = "email";
        public const string Gender = "gender";
        public const string ZoneInfo = "zoneinfo";
        public const string Locale = "locale";
        public const string Audience = "aud";
        public const string Issuer = "iss";
        public const string IssuedAt = "iat";
        public const string Expiration = "exp";
        public const string SessionId = "sid";
        public const string AuthenticationMethod = "amr";
        public const string JwtId = "jti";
        public const string ClientId = "client_id";
        public const string Scope = "scope";
        public const string Role = "role";
        public const string NotBefore = "nbf";
        public const string Type = "typ";
    }
}
