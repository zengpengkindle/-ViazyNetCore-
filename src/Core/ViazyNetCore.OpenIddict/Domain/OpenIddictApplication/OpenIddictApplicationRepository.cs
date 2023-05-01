using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;

namespace ViazyNetCore.OpenIddict.Domain
{
    public class OpenIddictApplicationRepository : DefaultRepository<OpenIddictApplication, long>, IOpenIddictApplicationRepository
    {
        public OpenIddictApplicationRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
