using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.Repository
{
    [Injection]
    public class ThridAuthUserRepository : DefaultRepository<ThridAuthUser, long>, IThridAuthUserRepository
    {
        public ThridAuthUserRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
