using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ViazyNetCore.Identity.Domain
{
    public partial class IdentityUserStore : IUserClaimStore<IdentityUser>
    {
        public async Task AddClaimsAsync(IdentityUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));
            Check.NotNull(claims, nameof(claims));

            //await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Claims, cancellationToken);

            user.AddClaims(claims);
        }

        public async Task<IList<Claim>> GetClaimsAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));
            var claims = await this._userClaimRepository.GetUserClaim(user.Id);
            user.AddClaims(claims.Select(c => c.ToClaim()));
            //await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Claims, cancellationToken);
            user.AddClaim(new Claim(IdentityClaimTypes.Subject, user.Id.ToString()));
            user.AddClaims(claims.Select(c => c.ToClaim()));
            return user.Claims.Select(c => c.ToClaim()).ToList();
        }

        public async Task<IList<IdentityUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(claim, nameof(claim));

            //return await this._userClaimRepository.GetListByClaimAsync(claim, cancellationToken: cancellationToken);
            throw new NotImplementedException();
        }

        public Task RemoveClaimsAsync(IdentityUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task ReplaceClaimAsync(IdentityUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));
            Check.NotNull(claim, nameof(claim));
            Check.NotNull(newClaim, nameof(newClaim));

            //await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Claims, cancellationToken);

            user.ReplaceClaim(claim, newClaim);
        }
    }
}
