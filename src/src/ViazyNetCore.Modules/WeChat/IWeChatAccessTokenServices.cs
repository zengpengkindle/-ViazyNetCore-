using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules
{
    [Injection]
    public interface IWeChatAccessTokenServices
    {
        Task InsertAsync(WeChatAccessToken entity);
        Task<WeChatAccessToken> QueryByClauseAsync(Expression<Func<WeChatAccessToken, bool>> predicate);
        Task UpdateAsync(WeChatAccessToken entity);
    }
}
