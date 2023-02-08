using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Modules.Repository;

namespace ViazyNetCore.Modules
{
    [Injection]
    public class SysTaskLogServices : ISysTaskLogServices
    {
        private readonly ISysTaskLogRepository _sysTaskLogRepository;

        public SysTaskLogServices(ISysTaskLogRepository sysTaskLogRepository)
        {
            this._sysTaskLogRepository = sysTaskLogRepository;
        }

        public Task InsertAsync(SysTaskLog log)
        {
            return this._sysTaskLogRepository.InsertAsync(log);
        }
    }
}
