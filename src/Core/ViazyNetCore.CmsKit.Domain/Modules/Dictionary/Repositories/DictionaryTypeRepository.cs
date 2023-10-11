using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;

namespace ViazyNetCore.CmsKit.Modules.Repositories
{
    [Injection]
    public class DictionaryTypeRepository : DefaultRepository<DictionaryType, long>, IDictionaryTypeRepository
    {
        public DictionaryTypeRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
