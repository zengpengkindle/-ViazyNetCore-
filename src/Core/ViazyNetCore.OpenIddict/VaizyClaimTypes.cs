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
        public const string TenantId = "tenantid";
        public static string UserName { get; set; } = ClaimTypes.Name;
        public static string EditionId { get; internal set; }
        public static string ImpersonatorTenantId { get; internal set; }
        public static string ImpersonatorUserId { get; internal set; }

        public static readonly string UserId = ClaimTypes.NameIdentifier; 
        internal static readonly string ClientId;
#pragma warning restore IDE1006 // 命名样式
    }
}
