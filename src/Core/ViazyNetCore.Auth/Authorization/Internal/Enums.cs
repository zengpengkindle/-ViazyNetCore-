using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Authorization
{
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
        Deleted = -1
    }

    public enum MenuType
    {
        /// <summary>
        /// 0:叶子节点 
        /// </summary>
        Node = 0,
        /// <summary>
        /// 1:中间节点
        /// </summary>
        MidNode = 1,
        /// <summary>
        ///  2:按钮
        /// </summary>
        Button = 2
    }
}
