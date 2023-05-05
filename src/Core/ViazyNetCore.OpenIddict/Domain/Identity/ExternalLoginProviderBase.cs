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
    public abstract class ExternalLoginProviderBase : IExternalLoginProvider
    {
        public ICurrentTenant CurrentTenant { get; }
        public IdentityUserManager UserManager { get; }
        public IOptions<IdentityOptions> IdentityOptions { get; }

        protected ExternalLoginProviderBase(
        ICurrentTenant currentTenant,
        IdentityUserManager userManager,
        IOptions<IdentityOptions> identityOptions)
        {
            this.CurrentTenant = currentTenant;
            this.UserManager = userManager;
            this.IdentityOptions = identityOptions;
        }

        public async Task<IdentityUser> CreateUserAsync(string userName, string providerName)
        {
            var externalUser = await GetUserInfoAsync(userName);
            return await CreateUserAsync(externalUser, userName, providerName);
        }

        protected abstract Task<ExternalLoginUserInfo> GetUserInfoAsync(string userName);

        protected virtual Task<ExternalLoginUserInfo> GetUserInfoAsync(IdentityUser user)
        {
            return GetUserInfoAsync(user.Username);
        }

        protected virtual async Task<IdentityUser> CreateUserAsync(ExternalLoginUserInfo externalUser, string userName, string providerName)
        {
            NormalizeExternalLoginUserInfo(externalUser, userName);

            var user = new IdentityUser
            {
                Username = userName,
                Nickname = externalUser.Surname
            };

            user.Username = externalUser.Name;

            await UserManager.CreateAsync(user);

            await UserManager.AddLoginAsync(
                        user,
                        new UserLoginInfo(
                            providerName,
                            externalUser.ProviderKey,
                            providerName
                        )
                    )
                ;

            return user;
        }

        public abstract Task<bool> IsEnabledAsync();

        public abstract Task<bool> TryAuthenticateAsync(string userName, string plainPassword);

        public Task UpdateUserAsync(IdentityUser user, string providerName)
        {
            throw new NotImplementedException();
        }
        private static void NormalizeExternalLoginUserInfo(
            ExternalLoginUserInfo externalUser,
            string userName
        )
        {
            if (externalUser.ProviderKey.IsNullOrWhiteSpace())
            {
                externalUser.ProviderKey = userName;
            }
        }

        protected virtual async Task UpdateUserAsync(IdentityUser user, ExternalLoginUserInfo externalUser, string providerName)
        {
            NormalizeExternalLoginUserInfo(externalUser, user.Username);
            if (!externalUser.Name.IsNullOrWhiteSpace())
            {
                user.Nickname = externalUser.Name;
            }
        }
    }
}
