using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ViazyNetCore.Model
{
    public class WeChatAccessToken:EntityBase
    {
        /// <summary>
        /// 类型1小程序2公众号
        /// </summary>
        public int AppType { get; set; }

        /// <summary>
        /// appId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// accessToken
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 第三方登录类型
        /// </summary>
        public UserAccountTypes? Type { get; set; }

        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime ExpireTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
