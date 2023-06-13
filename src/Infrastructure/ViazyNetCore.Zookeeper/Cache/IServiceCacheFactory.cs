using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Zookeeper.Cache
{
    public interface IServiceCacheFactory
    {
        /// <summary>
        /// 根据服务路由描述符创建服务路由。
        /// </summary>
        /// <param name="descriptors">服务路由描述符。</param>
        /// <returns>服务路由集合。</returns>
        Task<IEnumerable<ServiceCache>> CreateServiceCachesAsync(IEnumerable<ServiceCacheDescriptor> descriptors);
    }
}
