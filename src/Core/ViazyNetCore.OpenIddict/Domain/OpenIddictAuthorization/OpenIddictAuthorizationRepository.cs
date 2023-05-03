using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;

namespace ViazyNetCore.OpenIddict.Domain
{
    public class OpenIddictAuthorizationRepository : DefaultRepository<OpenIddictAuthorization, long>, IOpenIddictAuthorizationRepository
    {
        public OpenIddictAuthorizationRepository(IFreeSql fsql) : base(fsql)
        {
        }

        public Task PruneAsync(DateTime date, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
