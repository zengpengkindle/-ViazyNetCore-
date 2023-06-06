using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ViazyNetCore.Redis
{
    public partial class RedisService : IRedisCache
    {
        private readonly RedisCacheOptions _options;
        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1);

        public volatile Lazy<IConnectionMultiplexer> RedisConnection;
        private readonly ILogger<RedisService> _logger;

        public RedisService(ILogger<RedisService> logger, IOptions<RedisCacheOptions> optionsAccessor)
        {
            _logger = logger;
            RedisConnection = new Lazy<IConnectionMultiplexer>(this.GetRedisConnection());
        }

        /// <summary>
        /// 获取Redis Connection
        /// </summary>
        /// <returns></returns>
        private IConnectionMultiplexer GetRedisConnection()
        {
            //如果已经连接实例，直接返回
            if (RedisConnection.Value != null && RedisConnection.Value.IsConnected)
            {
                return RedisConnection.Value;
            }
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


        public async Task Clear()
        {
            foreach (var endPoint in this.RedisConnection.Value.GetEndPoints())
            {
                var server = this.RedisConnection.Value.GetServer(endPoint);
                foreach (var key in server.Keys())
                {
                    await this.GetDatabase().KeyDeleteAsync(key);
                }
            }
        }

        public async Task<bool> Exist(string key)
        {
            return await this.GetDatabase().KeyExistsAsync(key);
        }

        public async Task<string?> Get(string key)
        {
            return await this.GetDatabase().StringGetAsync(key);
        }

        public async Task Remove(string key)
        {
            await this.GetDatabase().KeyDeleteAsync(key);
        }

        public async Task Set(string key, object value, TimeSpan cacheTime)
        {
            if (value != null)
            {
                //序列化，将object值生成RedisValue
                await this.GetDatabase().StringSetAsync(key, JsonConvert.SerializeObject(value), cacheTime);
            }
        }

        public async Task<TEntity?> Get<TEntity>(string key)
        {
            var value = await this.GetDatabase().StringGetAsync(key);
            if (value.HasValue)
            {
                //需要用的反序列化，将Redis存储的Byte[]，进行反序列化
                return JsonConvert.DeserializeObject<TEntity>(value!);
            }
            else
            {
                return default;
            }
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
