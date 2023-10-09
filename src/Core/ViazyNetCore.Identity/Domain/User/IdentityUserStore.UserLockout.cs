using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ViazyNetCore.Caching;
using ViazyNetCore.Data.FreeSql.Extensions;

namespace ViazyNetCore.Identity.Domain
{
    public partial class IdentityUserStore : IUserLockoutStore<IdentityUser>
    {
        public async Task<int> GetAccessFailedCountAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Check.NotNull(user, nameof(user));

            var checkDto = await this.GetUserLockoutAsync(user.Username, cancellationToken);
            return checkDto.ErrorCount;
        }

        public Task<bool> GetLockoutEnabledAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Check.NotNull(user, nameof(user));
            return Task.FromResult(true);
        }

        public async Task<DateTimeOffset?> GetLockoutEndDateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Check.NotNull(user, nameof(user));

            var checkDto = await this.GetUserLockoutAsync(user.Username, cancellationToken);
            return checkDto.LastForbiddenTime;
        }

        public async Task<int> IncrementAccessFailedCountAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Check.NotNull(user, nameof(user));

            var userLoginCheck = await this.GetUserLockoutAsync(user.Username, cancellationToken);
            var cacheKey = this.GetUsernameCacheKey(user.Username);

            userLoginCheck.ErrorCount += 1;
            this._cacheService.Set(cacheKey, userLoginCheck, CachingExpirationType.RelativelyStable);

            return userLoginCheck.ErrorCount;
        }

        public Task ResetAccessFailedCountAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            var cacheKey = this.GetUsernameCacheKey(user.Username);
            this._cacheService.Remove(cacheKey);
            return Task.CompletedTask;
        }

        public Task SetLockoutEnabledAsync(IdentityUser user, bool enabled, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task SetLockoutEndDateAsync(IdentityUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Check.NotNull(user, nameof(user));

            var userLoginCheck = await this.GetUserLockoutAsync(user.Username, cancellationToken);
            var cacheKey = this.GetUsernameCacheKey(user.Username);

            userLoginCheck.LastForbiddenTime = DateTime.Now;
            this._cacheService.Set(cacheKey, userLoginCheck, CachingExpirationType.RelativelyStable);

        }

        private string GetUsernameCacheKey(string username)
        {
            var result = string.Format("Login_CACHE_Username_{0}", username);
            return result;
        }

        private async Task<UserLoginCheckDto> GetUserLockoutAsync(string username, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Check.NotNullOrWhiteSpace(username, nameof(username));

            var cacheKey = this.GetUsernameCacheKey(username);
            var result = this._cacheService.GetFromFirstLevel<UserLoginCheckDto>(cacheKey);
            if (result == null)
            {
                return new UserLoginCheckDto
                {
                    ErrorCount = 0,
                };
            }
            return result;
        }
    }
}
