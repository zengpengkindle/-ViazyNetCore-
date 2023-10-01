using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ViazyNetCore.Authorization.Models;

namespace ViazyNetCore.Identity.Domain
{
    public class IdentityUser : BmsUser
    {
        public bool TwoFactorEnabled { get; set; }
        public string SecurityStamp { get; internal set; }

        public virtual ICollection<IdentityUserToken> Tokens { get; protected set; } = new List<IdentityUserToken>();

        /// <summary>
        /// Navigation property for the claims this user possesses.
        /// </summary>
        public virtual ICollection<IdentityUserClaim> Claims { get; protected set; } = new List<IdentityUserClaim>();

        public virtual void SetToken(string loginProvider, string name, string value)
        {
            var token = FindToken(loginProvider, name);
            if (token == null)
            {
                this.Tokens.Add(new IdentityUserToken(Id, loginProvider, name, value, TenantId));
            }
            else
            {
                token.Value = value;
            }
        }
        public virtual IdentityUserToken? FindToken(string loginProvider, string name)
        {
            return this.Tokens.FirstOrDefault(t => t.LoginProvider == loginProvider && t.Name == name);
        }


        public virtual void AddClaims(IEnumerable<Claim> claims)
        {
            Check.NotNull(claims, nameof(claims));

            foreach (var claim in claims)
            {
                AddClaim(claim);
            }
        }
        public virtual void AddClaim(Claim claim)
        {
            Check.NotNull(claim, nameof(claim));
            Claims.Add(new IdentityUserClaim(Id, claim, TenantId));
        }
        public virtual void ReplaceClaim([NotNull] Claim claim, [NotNull] Claim newClaim)
        {
            Check.NotNull(claim, nameof(claim));
            Check.NotNull(newClaim, nameof(newClaim));

            var userClaims = Claims.Where(uc => uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type);
            foreach (var userClaim in userClaims)
            {
                userClaim.SetClaim(newClaim);
            }
        }
    }
}
