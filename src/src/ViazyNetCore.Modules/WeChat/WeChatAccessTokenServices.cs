using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Modules.Repository;

namespace ViazyNetCore.Modules
{
    [Injection]
    public class WeChatAccessTokenServices : IWeChatAccessTokenServices
    {
        private readonly IWeChatAccessTokenRepository _weChatAccessTokenRepository;

        public WeChatAccessTokenServices(IWeChatAccessTokenRepository weChatAccessTokenRepository)
        {
            this._weChatAccessTokenRepository = weChatAccessTokenRepository;
        }

        public Task InsertAsync(WeChatAccessToken entity)
        {
            return this._weChatAccessTokenRepository.InsertAsync(entity);
        }

        public Task<WeChatAccessToken> QueryByClauseAsync(Expression<Func<WeChatAccessToken, bool>> predicate)
        {
            return this._weChatAccessTokenRepository.Where(predicate).ToOneAsync();
        }

        public Task UpdateAsync(WeChatAccessToken entity)
        {
            return this._weChatAccessTokenRepository.UpdateAsync(entity);
        }
    }
}
