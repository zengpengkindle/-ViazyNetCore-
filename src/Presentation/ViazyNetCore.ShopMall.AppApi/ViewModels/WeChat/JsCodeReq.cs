using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.ShopMall.AppApi.ViewModels
{
    public class JsCodeReq
    {
        /// <summary>
        /// 通过 wx.login() 用户登录凭证（有效期五分钟）
        /// </summary>
        public string Code { get; set; }
    }
}
