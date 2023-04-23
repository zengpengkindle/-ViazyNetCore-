using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;
using ViazyNetCore.Model.Crm;

namespace ViazyNetCore.Modules.Repository
{
    public class BusinessRepository : DefaultRepository<CrmBusiness, long>, IBusinessRepository
    {
        public BusinessRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
