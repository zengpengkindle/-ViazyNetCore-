using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.Repository
{
    [Injection]
    public class SysTaskLogRepository : DefaultRepository<SysTaskLog, long>, ISysTaskLogRepository
    {
        public SysTaskLogRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
