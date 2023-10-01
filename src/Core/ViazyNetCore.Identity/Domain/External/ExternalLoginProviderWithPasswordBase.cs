using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ViazyNetCore.MultiTenancy;

namespace ViazyNetCore.Identity.Domain
{
    public abstract class ExternalLoginProviderWithPasswordBase : ExternalLoginProviderBase, IExternalLoginProviderWithPassword
    {
        public ExternalLoginProviderWithPasswordBase(ICurrentTenant currentTenant
            , IdentityUserManager userManager
            , IOptions<IdentityOptions> identityOptions
            , bool canObtainUserInfoWithoutPassword = false) 
            : base(currentTenant, userManager, identityOptions)
        {
            CanObtainUserInfoWithoutPassword = canObtainUserInfoWithoutPassword;
        }

        public bool CanObtainUserInfoWithoutPassword { get; }

        public async Task<IdentityUser> CreateUserAsync(string userName, string providerName, string plainPassword)
        {
            if (CanObtainUserInfoWithoutPassword)
            {
                return await CreateUserAsync(userName, providerName);
            }

            var externalUser = await GetUserInfoAsync(userName, plainPassword);

            return await CreateUserAsync(externalUser, userName, providerName);
        }

        public async Task UpdateUserAsync(IdentityUser user, string providerName, string plainPassword)
        {
            if (CanObtainUserInfoWithoutPassword)
            {
                await UpdateUserAsync(user, providerName);
                return;
            }

            var externalUser = await GetUserInfoAsync(user, plainPassword);

            await UpdateUserAsync(user, externalUser, providerName);
        }

        protected override Task<ExternalLoginUserInfo> GetUserInfoAsync(string userName)
        {
            throw new NotImplementedException();
        }
        protected abstract Task<ExternalLoginUserInfo> GetUserInfoAsync(string userName, string plainPassword);

        protected virtual Task<ExternalLoginUserInfo> GetUserInfoAsync(IdentityUser user, string plainPassword)
        {
            return GetUserInfoAsync(user.Username, plainPassword);
        }
    }
}
