using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore
{
    public enum Gender
    {
        [Description("男性")]
        Male = 1,
        [Description("女性")]
        Female = 2,
        [Description("未知")]
        UnKnown = 0,
    }

    /// <summary>
    /// 表示一个状态。
    /// </summary>
    public enum ComStatus
    {
        /// <summary>
        /// 表示实体已启用（1）。
        /// </summary>
        Enabled = 1,
        /// <summary>
        /// 表示实体已禁用（0）。
        /// </summary>
        Disabled = 0,
        /// <summary>
        /// 表示实体已被逻辑删除（-1）。
        /// </summary>
        Deleted = -1,
        /// <summary>
        /// 表示实体未审核(2)
        /// </summary>
        UnChecked = 2
    }
}
