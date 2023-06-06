using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace ViazyNetCore.Redis
{
    /// <summary>
    /// Redis缓存接口
    /// </summary>
    public interface IRedisCache : IRedisListCache, IRedisSetCache
    {

        //获取 Reids 缓存值
        Task<string?> Get(string key);

        //获取值，并序列化
        Task<TEntity?> Get<TEntity>(string key);

        //保存
        Task Set(string key, object value, TimeSpan cacheTime);

        //判断是否存在
        Task<bool> Exist(string key);

        //移除某一个缓存值
        Task Remove(string key);

        //全部清除
        Task Clear();

        /// <summary>
        /// 有序集合/定时任务延迟队列用的多
        /// </summary>
        /// <param name="redisKey">key</param>
        /// <param name="redisValue">元素</param>
        /// <param name="score">分数</param>
        Task SortedSetAddAsync(string redisKey, string redisValue, double score);

        IDictionary<string, object> GetAll(IEnumerable<string> keys);

        void RemoveCacheAll();

        void RemoveCacheRegex(string pattern);

        IList<string> SearchCacheRegex(string pattern);
    }
}
