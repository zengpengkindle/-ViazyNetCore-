using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.Repository
{
    [Injection]
    public interface ICmsUserWeChatRepository : IBaseRepository<CmsUserWeChat, long>
    {

    }
}
