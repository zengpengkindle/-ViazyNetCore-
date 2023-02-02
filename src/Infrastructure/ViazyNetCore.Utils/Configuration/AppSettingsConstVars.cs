﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Configuration
{
    public class AppSettingsConstVars
    {
        /// <summary>
        /// 获取redis连接字符串
        /// </summary>
        public static readonly string RedisConfigConnectionString = AppSettingsHelper.GetContent("RedisConfig", "ConnectionString");
    }
}