using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.OpenIddict.Domain
{
    public class IdentityClaim : EntityUpdate<long>
    {
        protected IdentityClaim() { }

        public IdentityClaim(Claim claim, long tenantId) : this(claim.Type, claim.Value, tenantId)
        {
        }
        protected internal IdentityClaim(string claimType, string claimValue, long tenantId)
        {
            Check.NotNull(claimType, nameof(claimType));

            ClaimType = claimType;
            ClaimValue = claimValue;
            this.TenantId = tenantId;
        }

        /// <summary>
        /// Gets or sets the claim type for this claim.
        /// </summary>
        public virtual string ClaimType { get; protected set; }

        /// <summary>
        /// Gets or sets the claim value for this claim.
        /// </summary>
        public virtual string ClaimValue { get; protected set; }
        public long TenantId { get; }

        /// <summary>
        /// Creates a Claim instance from this entity.
        /// </summary>
        /// <returns></returns>
        public virtual Claim ToClaim()
        {
            return new Claim(ClaimType, ClaimValue);
        }

        public virtual void SetClaim([NotNull] Claim claim)
        {
            Check.NotNull(claim, nameof(claim));

            ClaimType = claim.Type;
            ClaimValue = claim.Value;
        }
    }

    public class IdentityUserClaim : IdentityClaim
    {
        public long UserId { get; set; }
        public IdentityUserClaim() 
        {
        }

        public IdentityUserClaim(long userId, Claim claim, long tenantId) : base(claim, tenantId)
        {
            this.UserId = userId;
        }
    }
}
