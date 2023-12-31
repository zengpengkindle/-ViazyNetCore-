﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Auth
{
    public class AuthUser : IUser
    {
        /// <summary>
        /// 认证授权中心用户Key
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 获取Client Id
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// token发布时间(秒时间搓)
        /// </summary>
        public long Nbf { get; set; }

        /// <summary>
        /// token过期时间(秒时间搓)
        /// </summary>
        public long Exp { get; set; }
        /// <summary>
        /// 授权类型(登录方式)
        /// </summary>
        public string Amr { get; set; }
        /// <summary>
        /// 登录账号
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }

        public AuthUserType IdentityType { get; set; }

        public bool IsModerated => false;

        public long TenantId { get; set; }
    }
}
