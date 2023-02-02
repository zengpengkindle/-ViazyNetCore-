using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace System
{
    public static class DistributedExtensions
    {
        public static T Get<T>(this IDistributedCache distributedCache, string key)
        {
            byte[] array = distributedCache.Get(key);
            if (array == null || array.Length == 0)
            {
                return default(T);
            }

            return array.Bytes2Object<T>();
        }
    }
}
