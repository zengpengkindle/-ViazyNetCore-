using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ViazyNetCore.MultiTenancy;

namespace ViazyNetCore.OpenIddict.Domain
{
    public class ViazyExternalLoginProvider : ExternalLoginProviderWithPasswordBase
    {
        public const string Name = "Viazy";
        public ViazyExternalLoginProvider(ICurrentTenant currentTenant
            , IdentityUserManager userManager
            , IOptions<IdentityOptions> identityOptions
            , bool canObtainUserInfoWithoutPassword = false) 
            : base(currentTenant, userManager, identityOptions, canObtainUserInfoWithoutPassword)
        {
        }

        public override Task<bool> IsEnabledAsync()
        {
            return Task.FromResult(true);
        }

        public override Task<bool> TryAuthenticateAsync(string userName, string plainPassword)
        {
            return Task.FromResult(
            userName == "ext_user" && plainPassword == "abc"
        );
        }

        protected override Task<ExternalLoginUserInfo> GetUserInfoAsync(string userName, string plainPassword)
        {
            if (userName != "ext_user")
            {
                throw new ArgumentException();
            }

            return Task.FromResult(
                new ExternalLoginUserInfo("ext_user@test.com")
                {
                    Name = "Test Name", //optional, if the provider knows it
                    Surname = "Test Surname", //optional, if the provider knows it
                    EmailConfirmed = true, //optional, if the provider knows it
                    TwoFactorEnabled = false, //optional, if the provider knows it
                    PhoneNumber = "123", //optional, if the provider knows it
                    PhoneNumberConfirmed = false, //optional, if the provider knows it
                    ProviderKey = "123" //The id of the user on the provider side
                }
            );
        }
    }
}
