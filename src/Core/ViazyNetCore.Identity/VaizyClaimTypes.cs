using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace System.Security.Principal
{
    public class IdentityClaimTypes
    {
#pragma warning disable IDE1006 // 命名样式
        public static string TenantId = "tenantid";
        public static string UserName { get; set; } = ClaimTypes.Name;
        public static string EditionId { get; internal set; }
        public static string ImpersonatorTenantId { get; internal set; }
        public static string ImpersonatorUserId { get; internal set; }

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
#pragma warning restore IDE1006 // 命名样式
    }
}
