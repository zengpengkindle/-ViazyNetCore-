﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ViazyNetCore.Authorization.Repositories;
using ViazyNetCore.Modules;

namespace ViazyNetCore.OpenIddict.Domain
{
    public class IdentityUserManager: UserManager<IdentityUser>
    {
        public IdentityUserManager(IdentityUserStore store
            , IOptions<IdentityOptions> optionsAccessor
            , IPasswordHasher<IdentityUser> passwordHasher
            , IEnumerable<IUserValidator<IdentityUser>> userValidators
            , IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators
            , ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors
            , IServiceProvider services, ILogger<UserManager<IdentityUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public async Task<IdentityUser> GetByIdAsync(long id)
        {
            var user = await this.Store.FindByIdAsync(id.ToString(), CancellationToken);
            return user;
        }

        public Task<bool> ShouldPeriodicallyChangePasswordAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }
    }
}
