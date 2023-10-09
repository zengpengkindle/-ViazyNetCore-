using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ViazyNetCore.Authorization.Modules;
using ViazyNetCore.Authorization.Repositories;
using ViazyNetCore.Caching;
using ViazyNetCore.Data.FreeSql.Extensions;
using ViazyNetCore.Identity.Domain.User.Repositories;
using ViazyNetCore.Modules;

namespace ViazyNetCore.Identity.Domain
{
    public partial class IdentityUserStore
        : IUserStore<IdentityUser>
    {
        private readonly IUserRepository _userRepository;
        private readonly IdentityUserClaimRepository _userClaimRepository;
        private readonly IUserService _userService;
        private readonly ICacheService _cacheService;
        private readonly RoleService _roleService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;

        public IdentityUserStore(IUserRepository userRepository
            , IdentityUserClaimRepository userClaimRepository
            , IUserService userService
            , ICacheService cacheService
            , RoleService roleService
            , IServiceProvider serviceProvider
            , IMapper mapper)
        {
            this._userRepository = userRepository;
            this._userClaimRepository = userClaimRepository;
            this._userService = userService;
            this._cacheService = cacheService;
            this._roleService = roleService;
            this._serviceProvider = serviceProvider;
            this._mapper = mapper;
        }

        public async Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            await this._userRepository.InsertAsync(user, cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            await this._userRepository.DeleteAsync(user.Id, cancellationToken);

            return IdentityResult.Success;
        }

        public void Dispose()
        {
        }

        public async Task<IdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await this._userService.GetUser(userId.ParseTo<long>());
            var result = this._mapper.Map<IdentityUser>(user);
            return result;
        }

        public async Task<IdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = await this._userService.GetUserByUserName(normalizedUserName);
            var result = this._mapper.Map<IdentityUser>(user);
            return result;
        }

        public Task<string> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            return Task.FromResult(user.Username);
        }

        public Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            return Task.FromResult(user.Username);
        }

        public Task SetNormalizedUserNameAsync(IdentityUser user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            //user.Nickname = normalizedName;

            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(IdentityUser user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            user.Username = userName;

            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await this._userRepository.UpdateAsync(user);
            await this._userRepository.SaveManyAsync(user, nameof(BmsUser.Claims));
            return IdentityResult.Success;
        }
    }
}
