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
    public class FormFieldRepository : DefaultRepository<FormField, long>, IFormFieldRepository
    {
        public FormFieldRepository(IFreeSql fsql) : base(fsql)
        {
        }

        public Task<List<FormField>> GetListByFormIdAsync(long formId)
        {
            return this.Select.Where(p => p.FormId == formId).OrderBy(p => p.Sorting).ToListAsync();
        }
    }
}
