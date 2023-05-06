using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FreeSql;
using ViazyNetCore.Data.FreeSql.Extensions;
using ViazyNetCore.OpenIddict.Domain;

namespace ViazyNetCore.Identity.Domain.User.Repositories
{
    public class IdentityUserClaimRepository : DefaultRepository<IdentityUserClaim, long>
    {
        public IdentityUserClaimRepository(IFreeSql fsql) : base(fsql)
        {
        }

        public Task<List<IdentityUserClaim>> GetUserClaim(long userId)
        {
            return this.Select.Where(p => p.UserId == userId).ToListAsync();
        }

        //public Task<IList<IdentityUser>> GetListByClaimAsync(Claim claim, CancellationToken cancellationToken)
        //{
        //    return this.Select.Where(p => p.ClaimType == claim.Type && p.ClaimValue == claim.Value).ToListAsync(cancellationToken);
        //}
    }
}
