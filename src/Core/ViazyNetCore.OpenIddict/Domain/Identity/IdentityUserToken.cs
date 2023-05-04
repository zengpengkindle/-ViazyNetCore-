using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql.DataAnnotations;

namespace ViazyNetCore.OpenIddict.Domain
{
    public class IdentityUserToken : ITenant
    {

        public IdentityUserToken(long id, string loginProvider, string name, string value, long tenantId)
        {
            this.UserId = id;
            this.LoginProvider = loginProvider;
            this.Name = name;
            this.Value = value;
            this.TenantId = tenantId;
        }

        [Column(IsPrimary =true)]
        public long UserId { get; set; }

        [Column(IsPrimary = true)]
        public string Name { get; set; }

        [Column(IsPrimary = true)]
        public virtual string LoginProvider { get; set; }

        public virtual string Value { get; set; }

        public long TenantId { get; set; }
    }
}
