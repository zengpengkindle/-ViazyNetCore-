using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ViazyNetCore.OpenIddict.Domain
{
    public class SignInManager : SignInManager<IdentityUser>
    {
        public SignInManager(
        IdentityUserManager userManager,
        IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<IdentityUser> claimsFactory,
        IOptions<IdentityOptions> optionsAccessor,
        ILogger<SignInManager<IdentityUser>> logger,
        IAuthenticationSchemeProvider schemes,
        IUserConfirmation<IdentityUser> confirmation,
        IOptions<ViazyIdentityOptions> options) : base(
        userManager,
        contextAccessor,
        claimsFactory,
        optionsAccessor,
        logger,
        schemes,
        confirmation)
        {
            AbpOptions = options.Value;
            _identityUserManager = userManager;
        }

        public ViazyIdentityOptions AbpOptions { get; }

        private readonly IdentityUserManager _identityUserManager;


        public override async Task<SignInResult> PasswordSignInAsync(
            string userName,
            string password,
            bool isPersistent,
            bool lockoutOnFailure)
        {
            foreach (var externalLoginProviderInfo in AbpOptions.ExternalLoginProviders.Values)
            {
                var externalLoginProvider = (IExternalLoginProvider)Context.RequestServices
                    .GetRequiredService(externalLoginProviderInfo.Type);

                if (await externalLoginProvider.TryAuthenticateAsync(userName, password))
                {
                    var user = await UserManager.FindByNameAsync(userName);
                    if (user == null)
                    {
                        if (externalLoginProvider is IExternalLoginProviderWithPassword externalLoginProviderWithPassword)
                        {
                            user = await externalLoginProviderWithPassword.CreateUserAsync(userName, externalLoginProviderInfo.Name, password);
                        }
                        else
                        {
                            user = await externalLoginProvider.CreateUserAsync(userName, externalLoginProviderInfo.Name);
                        }
                    }
                    else
                    {
                        if (externalLoginProvider is IExternalLoginProviderWithPassword externalLoginProviderWithPassword)
                        {
                            await externalLoginProviderWithPassword.UpdateUserAsync(user, externalLoginProviderInfo.Name, password);
                        }
                        else
                        {
                            await externalLoginProvider.UpdateUserAsync(user, externalLoginProviderInfo.Name);
                        }
                    }

                    return await SignInOrTwoFactorAsync(user, isPersistent);
                }
            }

            return await base.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
        }

        protected override async Task<SignInResult> PreSignInCheck(IdentityUser user)
        {
            if (user.IsModerated)
            {
                Logger.LogWarning($"The user is not active therefore cannot login! (username: \"{user.Username}\", id:\"{user.Id}\")");
                return SignInResult.NotAllowed;
            }

            //if (user.ShouldChangePasswordOnNextLogin)
            //{
            //    Logger.LogWarning($"The user should change password! (username: \"{user.Username}\", id:\"{user.Id}\")");
            //    return SignInResult.NotAllowed;
            //}

            if (await _identityUserManager.ShouldPeriodicallyChangePasswordAsync(user))
            {
                return SignInResult.NotAllowed;
            }

            return await base.PreSignInCheck(user);
        }
    }
}
