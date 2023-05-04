using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Authorization.Models;

namespace ViazyNetCore.OpenIddict.Domain
{
    public class IdentityUser : BmsUser
    {
        public bool TwoFactorEnabled { get; set; }
        public string SecurityStamp { get; internal set; }


        public virtual ICollection<IdentityUserToken> Tokens { get; protected set; }

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
    }
}
