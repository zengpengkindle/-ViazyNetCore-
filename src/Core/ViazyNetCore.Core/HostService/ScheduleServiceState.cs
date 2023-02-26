using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// 表示一个服务状态。
    /// </summary>
    public class ScheduleServiceState
    {        /// <summary>
             /// 获取或设置一个值，表示是否已超时。
             /// </summary>
        public bool IsTimeout { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示取消标识。
        /// </summary>
        public CancellationToken CancellationToken { get; set; }

        /// <summary>
        /// 获取一个值，表示服务的当前执行环境是否有效。
        /// </summary>
        /// <returns>有效返回 <see langword="true"/> 值，否则返回 <see langword="false"/> 值。</returns>
        public bool Valid() => !this.IsTimeout;
    }
}
