using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules
{
    [Injection]
    public interface ISysTaskLogServices
    {
        Task InsertAsync(SysTaskLog log);
    }
}
