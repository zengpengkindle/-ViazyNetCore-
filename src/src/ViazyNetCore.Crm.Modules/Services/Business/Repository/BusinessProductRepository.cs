using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;
using ViazyNetCore.Model.Crm;

namespace ViazyNetCore.Modules.Repository
{
    public class BusinessProductRepository : DefaultRepository<CrmBusinessProduct, long>, IBusinessProductRepository
    {
        public BusinessProductRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
