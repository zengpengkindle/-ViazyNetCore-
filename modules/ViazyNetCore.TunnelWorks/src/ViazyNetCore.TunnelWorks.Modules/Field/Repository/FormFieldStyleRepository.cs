using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;
using ViazyNetCore.TunnelWorks.Models;

namespace ViazyNetCore.TunnelWorks.Modules.Repository
{
    public class FormFieldStyleRepository : DefaultRepository<FormFieldStyle, long>, IFormFieldStyleRepository
    {
        public FormFieldStyleRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
