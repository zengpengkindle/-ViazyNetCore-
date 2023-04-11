using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Modules.Models;

namespace ViazyNetCore.Modules
{
    [Injection]
    public interface IWechatAuthService
    {
        /// <summary>
        /// 微信授权
        /// </summary>
        Task<WechatAuthDto> Auth(WechatAuthUpdateDto updateDto);
    }
}
