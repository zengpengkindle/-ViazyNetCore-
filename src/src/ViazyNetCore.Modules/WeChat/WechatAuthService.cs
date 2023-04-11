using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Modules.Models;

namespace ViazyNetCore.Modules
{
    [Injection]
    public class WechatAuthService : IWechatAuthService
    {
        private readonly IThridAuthUserRepository _thridAuthUserRepository;
        private readonly IMemberThridAccountRepository _memberThridAccountRepository;

        public WechatAuthService(IThridAuthUserRepository thridAuthUserRepository
            , IMemberThridAccountRepository memberThridAccountRepository)
        {
            this._thridAuthUserRepository = thridAuthUserRepository;
            this._memberThridAccountRepository = memberThridAccountRepository;
        }

        public async Task<WechatAuthDto> Auth(WechatAuthUpdateDto updateDto)
        {
            var thridAuthUser = await this._thridAuthUserRepository.Where(p => p.UnionId == updateDto.UnionId).FirstAsync();
            if (thridAuthUser == null)
            {
                return new WechatAuthDto
                {
                    IsBindMobile = false,
                    OpenId = updateDto.OpenId,
                    AuthCode = null
                };
            }

            return new WechatAuthDto()
            {
            };
        }
    }
}
