﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Auth.Jwt
{
    public class JwtTokenResult
    {
        /// <summary>
        /// 获取到的凭证
        /// </summary>
        public string? AccessToken { get; set; } = null;

        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        public long ExpiresIn { get; set; }
    }
}
