using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;

namespace ViazyNetCore.OpenIddict.Domain
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
    }
}
