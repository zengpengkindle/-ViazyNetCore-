using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ViazyNetCore.Authorization;
using ViazyNetCore.Authorization.Modules;
using ViazyNetCore.Authorization.Repositories;
using ViazyNetCore.Modules;

namespace ViazyNetCore.OpenIddict.Domain
{
    [Obsolete]
    public class IdentityUserStore2 :
        IUserLoginStore<IdentityUser>,
        IUserRoleStore<IdentityUser>,
        IUserClaimStore<IdentityUser>,
        IUserPasswordStore<IdentityUser>,
        IUserSecurityStampStore<IdentityUser>,
        IUserEmailStore<IdentityUser>,
        IUserLockoutStore<IdentityUser>,
        IUserPhoneNumberStore<IdentityUser>,
        IUserTwoFactorStore<IdentityUser>,
        IUserAuthenticationTokenStore<IdentityUser>,
        IUserAuthenticatorKeyStore<IdentityUser>,
        IUserTwoFactorRecoveryCodeStore<IdentityUser>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        //private readonly RoleService _roleService;
        private readonly IOrgRepository _orgRepository;
        private readonly IMapper _mapper;
        private const string InternalLoginProvider = "[AspNetUserStore]";
        private const string AuthenticatorKeyTokenName = "AuthenticatorKey";
        private const string RecoveryCodeTokenName = "RecoveryCodes";

        private IdentityErrorDescriber ErrorDescriber => new IdentityErrorDescriber();

        public IdentityUserStore2(IUserRepository userRepository
            , IUserService userService
            //, RoleService roleService
            , IOrgRepository orgRepository
            , IMapper mapper)
        {
            this._userRepository = userRepository;
            this._userService = userService;
            //this._roleService = roleService;
            this._orgRepository = orgRepository;
            this._mapper = mapper;
        }
        public Task AddClaimsAsync(IdentityUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));
            Check.NotNull(claims, nameof(claims));

            //this._userRepository.Where(p=>p.Id==user.Id)

            throw new NotImplementedException();
        }

        public Task AddLoginAsync(IdentityUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));
            Check.NotNull(login, nameof(login));

            throw new NotImplementedException();
        }

        public Task AddToRoleAsync(IdentityUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<int> CountCodesAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            var mergedCodes = await GetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, cancellationToken) ?? "";
            if (mergedCodes.Length > 0)
            {
                return mergedCodes.Split(';').Length;
            }

            return 0;
        }

        public async Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            await this._userRepository.InsertAsync(user, cancellationToken);

            return IdentityResult.Success;
        }

        public Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        public Task<IdentityUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await this._userService.GetUser(userId.ParseTo<long>());
            return this._mapper.Map<IdentityUser>(user);
        }

        public Task<IdentityUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(loginProvider, nameof(loginProvider));
            Check.NotNull(providerKey, nameof(providerKey));
            throw new NotImplementedException();
        }

        public Task<IdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetAccessFailedCountAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetAuthenticatorKeyAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return GetTokenAsync(user, InternalLoginProvider, AuthenticatorKeyTokenName, cancellationToken);
        }

        public Task<IList<Claim>> GetClaimsAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetEmailAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            throw new NotSupportedException();
            //return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            return Task.FromResult(false);
        }

        public Task<bool> GetLockoutEnabledAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            return Task.FromResult(user.IsModerated);
        }

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedEmailAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public Task<string> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            return Task.FromResult(user.Username);
        }

        public Task<string> GetPasswordHashAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPhoneNumberAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            return Task.FromResult(user.Username);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            return Task.FromResult(true);
        }

        public Task<IList<string>> GetRolesAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetSecurityStampAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            return Task.FromResult(user.SecurityStamp);
        }

        public Task<string> GetTokenAsync(IdentityUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetTwoFactorEnabledAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<IdentityUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<IdentityUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<int> IncrementAccessFailedCountAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(IdentityUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RedeemCodeAsync(IdentityUser user, string code, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveClaimsAsync(IdentityUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(IdentityUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(IdentityUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveTokenAsync(IdentityUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ReplaceClaimAsync(IdentityUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ReplaceCodesAsync(IdentityUser user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken)
        {
            var mergedCodes = string.Join(";", recoveryCodes);
            return SetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, mergedCodes, cancellationToken);
        }

        public Task ResetAccessFailedCountAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetAuthenticatorKeyAsync(IdentityUser user, string key, CancellationToken cancellationToken)
        {
            return SetTokenAsync(user, InternalLoginProvider, AuthenticatorKeyTokenName, key, cancellationToken);
        }

        public Task SetEmailAsync(IdentityUser user, string email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(IdentityUser user, bool confirmed, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEnabledAsync(IdentityUser user, bool enabled, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(IdentityUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedEmailAsync(IdentityUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(IdentityUser user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            user.Username = normalizedName;

            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetPhoneNumberAsync(IdentityUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetPhoneNumberConfirmedAsync(IdentityUser user, bool confirmed, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetSecurityStampAsync(IdentityUser user, string stamp, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task SetTokenAsync(IdentityUser user, string loginProvider, string name, string value, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            //await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Tokens, cancellationToken);

            user.SetToken(loginProvider, name, value);
        }

        public Task SetTwoFactorEnabledAsync(IdentityUser user, bool enabled, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            user.TwoFactorEnabled = enabled;

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

            Check.NotNull(user, nameof(user));

            try
            {
                await this._userRepository.UpdateAsync(user, cancellationToken);
            }
            catch (DbException ex)
            {
                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
            }

            return IdentityResult.Success;
        }
    }
}
