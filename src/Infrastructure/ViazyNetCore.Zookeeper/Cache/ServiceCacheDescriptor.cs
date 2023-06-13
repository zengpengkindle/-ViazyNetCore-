using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Zookeeper.Cache
{
    public class ServiceCacheDescriptor
    {
        /// <summary>
        /// 服务地址描述符集合。
        /// </summary>
        public IEnumerable<CacheEndpointDescriptor> AddressDescriptors { get; set; }

        /// <summary>
        /// 缓存描述符。
        /// </summary>
        public CacheDescriptor CacheDescriptor { get; set; }
    }
}
