using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Modules;

namespace ViazyNetCore.Authorization.Repositories
{
    [Injection]
    public class OrgRepository : DefaultRepository<BmsOrg, long>, IOrgRepository
    {
        public OrgRepository(IFreeSql fsql) : base(fsql)
        {
        }

        ///<inheritdoc/>
        public async Task<List<long>> GetChildIdListAsync(long id)
        {
            return await Select
            .Where(a => a.Id == id)
            .AsTreeCte()
            .ToListAsync(a => a.Id);
        }
    }
}
