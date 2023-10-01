using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FreeSql;
using ViazyNetCore.Data.FreeSql.Extensions;
using ViazyNetCore.OpenIddict.Domain;

namespace ViazyNetCore.Identity.Domain.User.Repositories
{
    [Injection]
    public class IdentityUserClaimRepository : DefaultRepository<IdentityUserClaim, long>
    {
        private readonly IMapper _mapper;

        public IdentityUserClaimRepository(IFreeSql fsql, IMapper mapper) : base(fsql)
        {
            this._mapper = mapper;
        }

        public Task<List<IdentityUserClaim>> GetUserClaim(long userId)
        {
            return this.Select.Where(p => p.UserId == userId).ToListAsync();
        }

        public async Task<IList<IdentityUser>> GetListByClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            var result = await this.Select.From<BmsUser>().InnerJoin((c, u) => c.UserId == u.Id)
                .Where(t => t.t1.ClaimType == claim.Type && t.t1.ClaimValue == claim.Value)
                .WithTempQuery(t => t.t2)
                .ToListAsync(cancellationToken);
            return this._mapper.Map<List<BmsUser>, List<IdentityUser>>(result);
        }
    }
}
