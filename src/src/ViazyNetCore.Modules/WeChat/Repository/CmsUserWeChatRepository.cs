using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.Repository
{
    [Injection]
    public class CmsUserWeChatRepository : DefaultRepository<CmsUserWeChat, long>, ICmsUserWeChatRepository
    {
        public CmsUserWeChatRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
