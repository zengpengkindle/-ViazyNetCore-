using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.CmsKit.Modules.Repository
{
    [Injection]
    public class OperationLogRepository : DefaultRepository<OperationLog, long>, IOperationLogRepository
    {
        public OperationLogRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
