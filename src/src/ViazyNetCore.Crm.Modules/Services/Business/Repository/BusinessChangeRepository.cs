using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;
using ViazyNetCore.Model.Crm;

namespace ViazyNetCore.Modules.Repository
{
    public class BusinessChangeRepository : DefaultRepository<CrmBusinessChange, long>, IBusinessChangeRepository
    {
        public BusinessChangeRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
