using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.Models
{
    public class WechatAuthDto
    {
        public long MemberId { get; set; }
        public bool GetUserProfile { get; set; }

        public string OpenId { get; set; }
        /// <summary>
        /// 是否绑定了手机
        /// </summary>
        public bool IsBindMobile { get; set; }

        /// <summary>
        /// 临时授权码
        /// </summary>
        public string AuthCode { get; set; }
    }

    public class WechatAuthModel
    {
        public string UnionId { get; set; }

        public string AppId { get; set; }

        public string OpenId { get; set; }
    }
}
