using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;
using ViazyNetCore.Model.Crm;

namespace ViazyNetCore.Modules.Repository
{
    public class FieldSortRepository : DefaultRepository<CrmFieldSort, long>, IFieldSortRepository
    {
        public FieldSortRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
