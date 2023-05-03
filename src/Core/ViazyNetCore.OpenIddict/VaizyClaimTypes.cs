using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace System.Security.Principal
{
    public class VaizyClaimTypes
    {
#pragma warning disable IDE1006 // 命名样式
        public static string TenantId = "tenantid";
        public static string UserName { get; set; } = ClaimTypes.Name;
        public static string EditionId { get; internal set; }
        public static string ImpersonatorTenantId { get; internal set; }
        public static string ImpersonatorUserId { get; internal set; }

        public static string UserId = ClaimTypes.NameIdentifier;
        public static string ClientId;
        public static string EmailVerified;
        public static string Email;
        public static string PhoneNumberVerified;
        public static string PhoneNumber;
        public static string SurName;
        public static string Name;
        public static string Role;
#pragma warning restore IDE1006 // 命名样式
    }
}
