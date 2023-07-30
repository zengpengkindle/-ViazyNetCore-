using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;
using ViazyNetCore.TunnelWorks.Models;

namespace ViazyNetCore.TunnelWorks.Modules.Repository
{
    [Injection]
    public interface IFormFieldRepository : IBaseRepository<FormField, long>
    {
        Task<List<FormField>> GetListByFormIdAsync(long formId);
    }
}
