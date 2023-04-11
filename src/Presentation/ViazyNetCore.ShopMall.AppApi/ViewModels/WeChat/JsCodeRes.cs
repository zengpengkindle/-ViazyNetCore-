using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ViazyNetCore.Auth.Jwt;

namespace ViazyNetCore.ShopMall.AppApi.ViewModels
{
    public class JsCodeRes
    {
        /// <summary>
        /// 小程序openid
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 是否需要唤起 wx.getUserProfile, 获取昵称和头像 
        /// </summary>
        public bool GetUserProfile { get; set; }

        /// <summary>
        /// 登录令牌
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public JwtTokenResult Token { get; set; }
        /// <summary>
        /// 1:注册 2:登录
        /// </summary>
        public int OpType { get; set; }

        /// <summary>
        /// 是否已绑定手机
        /// </summary>
        public bool IsBindMobile { get; set; }

        /// <summary>
        /// 临时令牌
        /// </summary>
        public string AuthCode { get; set; }
    }
}
