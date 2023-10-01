using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ViazyNetCore.Authorization.Modules;
using ViazyNetCore.Authorization;
using ViazyNetCore.Modules;

namespace ViazyNetCore.Identity.Domain
{
    public class IdentityUserManager : UserManager<IdentityUser>
    {
        private readonly ViazyIdentityOptions _identityOptions;
        private readonly IUserService _userService;

        public IdentityUserManager(IdentityUserStore store
            , IOptions<IdentityOptions> optionsAccessor
            , IOptions<ViazyIdentityOptions> identityOptions
            , IPasswordHasher<IdentityUser> passwordHasher
            , IEnumerable<IUserValidator<IdentityUser>> userValidators
            , IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators
            , ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors
            , IUserService userService
            , IServiceProvider services, ILogger<UserManager<IdentityUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            this._identityOptions = identityOptions.Value;
            this._userService = userService;
        }

        public async Task<IdentityUser> GetByIdAsync(long id)
        {
            var user = await this.Store.FindByIdAsync(id.ToString(), CancellationToken);
            return user;
        }

        public Task<bool> ShouldPeriodicallyChangePasswordAsync(IdentityUser user)
        {
            return Task.FromResult(false);
        }

        public override Task<bool> CheckPasswordAsync(IdentityUser user, string password)
        {
            //if (UserPasswordHelper.CheckPassword(password, user.Password, user.PasswordSalt, this._identityOptions.UserPasswordFormat))
            //{
            //    return Task.FromResult(true);
            //}
            //return Task.FromResult(false);
            return base.CheckPasswordAsync(user, password);
        }

        public override async Task<IdentityResult> AddPasswordAsync(IdentityUser user, string password)
        {
            var result = await base.AddPasswordAsync(user, password);
            return result;
        }

        public override Task<IdentityResult> CreateAsync(IdentityUser user, string password)
        {
            return base.CreateAsync(user, password);
        }

        public override Task<IdentityResult> CreateAsync(IdentityUser user)
        {
            return base.CreateAsync(user);
        }

        public override Task<IdentityResult> UpdateAsync(IdentityUser user)
        {
            return base.UpdateAsync(user);
        }
    }
}
