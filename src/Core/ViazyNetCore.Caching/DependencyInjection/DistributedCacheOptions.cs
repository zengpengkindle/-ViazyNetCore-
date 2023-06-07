using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Caching
{
    public class DistributedCacheOptions
    {
        /// <summary>
        /// 缓存过期系数
        /// </summary>
        public float CacheExpirationFactor { get; set; } = 1;
        /// <summary>
        /// 是否启用分布式缓存
        /// </summary>
        public bool EnableDistributedCache { get; set; } = true;
    }
}
