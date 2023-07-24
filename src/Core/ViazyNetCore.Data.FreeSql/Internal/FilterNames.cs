﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Data.FreeSql
{
    public class FilterNames
    {
        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        public const string Delete = "Delete";

        /// <summary>
        /// 租户
        /// </summary>
        [Description("租户")]
        public const string Tenant = "Tenant";

        /// <summary>
        /// 本人权限
        /// </summary>
        [Description("本人权限")]
        public const string Self = "Self";

        /// <summary>
        /// 数据权限
        /// </summary>
        [Description("数据权限")]
        public const string Data = "Data";

        /// <summary>
        /// 会员
        /// </summary>
        [Description("会员")]
        public const string Member = "Member";
    }
}
