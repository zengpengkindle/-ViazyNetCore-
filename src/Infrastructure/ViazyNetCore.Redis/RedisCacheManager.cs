using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace ViazyNetCore.Redis
{
    public class RedisCacheManager
    {
        private readonly RedisCacheOptions _options;
        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1);

        public volatile Lazy<IConnectionMultiplexer> RedisConnection;

        public RedisCacheManager(IOptions<RedisCacheOptions> optionsAccessor)
        {
            this._options = optionsAccessor.Value;
            RedisConnection = new Lazy<IConnectionMultiplexer>(this.GetRedisConnection());
        }

        /// <summary>
        /// 核心代码，获取连接实例
        /// 通过双if 夹lock的方式，实现单例模式
        /// </summary>
        /// <returns></returns>
        private IConnectionMultiplexer GetRedisConnection()
        {
            //如果已经连接实例，直接返回
            if (RedisConnection.Value != null && RedisConnection.Value.IsConnected)
            {
                return RedisConnection.Value;
            }
            //加锁，防止异步编程中，出现单例无效的问题
            _connectionLock.Wait();
            try
            {
                if (RedisConnection.Value != null)
                {
                    //释放redis连接
                    return this.RedisConnection.Value;
                }

                if (_options.ConnectionMultiplexerFactory == null)
                {
                    if (_options.ConfigurationOptions != null)
                    {
                        return ConnectionMultiplexer.Connect(_options.ConfigurationOptions);
                    }
                    else
                    {
                        return ConnectionMultiplexer.Connect(_options.Configuration);
                    }
                }
                else
                {
                    return _options.ConnectionMultiplexerFactory!().GetAwaiter().GetResult();
                }
            }
            finally
            {
                _connectionLock.Release();
            }
        }

        private IDatabase GetDatabase()
        {
            return this.RedisConnection.Value.GetDatabase();
        }

        /// <summary>
        /// 判断key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            return this.GetDatabase().KeyExists(key);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresIn">缓存时间</param>
        /// <returns></returns>
        public bool Set(string key, object value, int expiresIn = 0)
        {
            if (value != null)
            {
                //序列化，将object值生成RedisValue
                if (expiresIn > 0)
                {
                    return this.GetDatabase().StringSet(key, JSON.Stringify(value), TimeSpan.FromMinutes(expiresIn));
                }
                else
                {
                    return this.GetDatabase().StringSet(key, JSON.Stringify(value));
                }
            }
            return false;
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public void Remove(string key)
        {
            this.GetDatabase().KeyDelete(key);
        }

        /// <summary>
        /// 批量删除缓存
        /// </summary>
        /// <returns></returns>
        public void RemoveAll(IEnumerable<string> keys)
        {
            foreach (var key in keys)
            {
                this.GetDatabase().KeyDelete(key);
            }

        }

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public T? Get<T>(string key)
        {
            var value = this.GetDatabase().StringGet(key);
            if (value.HasValue)
            {
                //需要用的反序列化，将Redis存储的Byte[]，进行反序列化
                return JSON.Parse<T>(value!);
            }

            return default;
        }

        public string? Get(string key)
        {
            return this.GetDatabase().StringGet(key);
        }

        public IDictionary<string, object> GetAll(IEnumerable<string> keys)
        {
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));
            var dict = new Dictionary<string, object>();

            keys.ToList().ForEach(item => dict.Add(item, this.GetDatabase().StringGet(item)));
            return dict;

        }

        public void RemoveCacheAll()
        {
            foreach (var endPoint in this.RedisConnection.Value.GetEndPoints())
            {
                var server = this.RedisConnection.Value.GetServer(endPoint);
                foreach (var key in server.Keys())
                {
                    this.GetDatabase().KeyDelete(key);
                }
            }
        }

        public void RemoveCacheRegex(string pattern)
        {
            var script = "return redis.call('keys',@pattern)";
            var prepared = LuaScript.Prepare(script);
            var redisResult = this.GetDatabase().ScriptEvaluate(prepared, new { pattern });
            if (!redisResult.IsNull)
            {
                this.GetDatabase().KeyDelete((RedisKey[])redisResult); //删除一组key
            }
        }

        public IList<string> SearchCacheRegex(string pattern)
        {
            var list = new List<string>();
            var script = "return redis.call('keys',@pattern)";
            var prepared = LuaScript.Prepare(script);
            var redisResult = this.GetDatabase().ScriptEvaluate(prepared, new { pattern });
            if (!redisResult.IsNull)
            {
                foreach (var key in (RedisKey[])redisResult!)
                {
                    var cacheKey = this.GetDatabase().StringGet(key);
                    if (cacheKey != RedisValue.Null)
                        list.Add(cacheKey!);
                }
            }
            return list;
        }
    }
}
