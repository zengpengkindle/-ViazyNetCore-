using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Auth.Authorization.ViewModels
{
    public class UserTokenModel
    {

        /// <summary>
        /// 获取到的凭证
        /// </summary>
        public string? AccessToken { get; set; } = null;

        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        public long ExpiresIn { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Nickname { get; set; }

        public string[] Permissions { get; set; }
    }
}
