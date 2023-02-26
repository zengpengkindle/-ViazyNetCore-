using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Authorization.Modules.Repositories
{
    [Injection]
    public class DictionaryValueRepository : DefaultRepository<DictionaryValue, long>, IDictionaryValueRepository
    {
        public DictionaryValueRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
