using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Caching
{
    //public class CSRedisCache : ICache
    //{
    //    public CSRedisCache()
    //    {
    //    }

    //    public void Clear()
    //    {
    //    }

    //    public object Get(string cacheKey)
    //    {
    //        return RedisHelper.Get(cacheKey);
    //    }

    //    public T Get<T>(string cacheKey)
    //    {
    //        return RedisHelper.Get<T>(cacheKey);
    //    }

    //    public void MarkDeletion(string cacheKey, object value, TimeSpan expiresIn)
    //    {
    //        this.Set(cacheKey, value, expiresIn);
    //        //this.Remove(cacheKey);
    //    }

    //    public void Remove(string cacheKey)
    //    {
    //        RedisHelper.Del(cacheKey);
    //    }

    //    public void Set(string cacheKey, object value, TimeSpan expiresIn)
    //    {
    //        RedisHelper.Set(cacheKey, value, expiresIn);
    //    }
    //}
}
