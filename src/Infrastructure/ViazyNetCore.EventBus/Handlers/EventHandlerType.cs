using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore
{
    public enum EventHandlerType
    {
        /// <summary>
        /// 触发模式
        /// </summary>
        Trigger = 1,
        /// <summary>
        /// 触发模式
        /// </summary>
        TriggerAsync = 2,
        /// <summary>
        /// 消息模式
        /// </summary>
        Topic = 10
    }
}
