using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;

namespace ViazyNetCore.OpenIddict.Domain
{
    public class OpenIddictScopeRepository : DefaultRepository<OpenIddictScope, long>, IOpenIddictScopeRepository
    {
        public OpenIddictScopeRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
