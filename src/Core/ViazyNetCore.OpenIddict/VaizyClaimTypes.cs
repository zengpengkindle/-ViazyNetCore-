using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using OpenIddict.Abstractions;

namespace System.Security.Principal
{
    public class VaizyClaimTypes
    {
#pragma warning disable IDE1006 // 命名样式
        public static string TenantId = "tenantid";
        public static string Subject { get; set; } = OpenIddictConstants.Claims.Subject;
        public static string UserName { get; set; } = ClaimTypes.Name;
        public static string EditionId { get; internal set; }
        public static string ImpersonatorTenantId { get; internal set; }
        public static string ImpersonatorUserId { get; internal set; }

        public static string UserId = ClaimTypes.NameIdentifier;
        public static string ClientId = OpenIddictConstants.Claims.ClientId;
        public static string EmailVerified= OpenIddictConstants.Claims.EmailVerified;
        public static string Email = OpenIddictConstants.Claims.Email;
        public static string PhoneNumberVerified= OpenIddictConstants.Claims.PhoneNumberVerified;
        public static string PhoneNumber = OpenIddictConstants.Claims.PhoneNumber;
        public static string SurName = ClaimTypes.Surname;
        public static string Name = ClaimTypes.Name;
        public static string Role = ClaimTypes.Role;
#pragma warning restore IDE1006 // 命名样式
    }
}
