using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Zookeeper.Web
{
    /// <summary>
    /// 服务描述符标记。
    /// </summary>
    public abstract class ServiceDescriptorAttribute : Attribute
    {
        /// <summary>
        /// 应用标记。
        /// </summary>
        /// <param name="descriptor">服务描述符。</param>
        public abstract void Apply(ServiceDescriptor descriptor);
    }
}
