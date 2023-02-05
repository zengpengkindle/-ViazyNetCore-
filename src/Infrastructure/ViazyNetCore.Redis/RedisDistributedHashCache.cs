using System.Net;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using ViazyNetCore.Caching;

namespace ViazyNetCore.Redis
{
    public class RedisDistributedHashCache : RedisCache, IDistributedHashCache
    {
        private readonly RedisCacheOptions _options;
        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1);
        private volatile IConnectionMultiplexer _connection;
        private readonly IRedisCache _redisCache;
        private IDatabase _cache;
        private bool _disposed;
        private string _setScript = "\r\n                redis.call('HSET', KEYS[1], 'absexp', ARGV[1], 'sldexp', ARGV[2], 'data', ARGV[4])\r\n                if ARGV[3] ~= '-1' then\r\n                  redis.call('EXPIRE', KEYS[1], ARGV[3])\r\n                end\r\n                return 1";

        private static readonly Version ServerVersionWithExtendedSetCommand = new Version(4, 0, 0);

        private readonly string _instance;

        public RedisDistributedHashCache(IOptions<RedisCacheOptions> optionsAccessor, IRedisCache redisCache) : base(optionsAccessor)
        {
            this._options = optionsAccessor.Value;
            this._redisCache = redisCache;
            this._instance = this._options.InstanceName ?? string.Empty;
        }

        private void Connect()
        {
            CheckDisposed();
            if (_cache != null)
            {
                return;
            }

            _connectionLock.Wait();
            try
            {
                if (_cache != null)
                {
                    return;
                }

                if (_options.ConnectionMultiplexerFactory == null)
                {
                    if (_options.ConfigurationOptions != null)
                    {
                        _connection = ConnectionMultiplexer.Connect(_options.ConfigurationOptions);
                    }
                    else
                    {
                        _connection = ConnectionMultiplexer.Connect(_options.Configuration);
                    }
                }
                else
                {
                    _connection = _options.ConnectionMultiplexerFactory!().GetAwaiter().GetResult();
                }

                PrepareConnection();
                _cache = _connection.GetDatabase();
            }
            finally
            {
                _connectionLock.Release();
            }
        }

        private async Task ConnectAsync(CancellationToken token = default(CancellationToken))
        {
            CheckDisposed();
            token.ThrowIfCancellationRequested();
            if (_cache != null)
            {
                return;
            }

            await _connectionLock.WaitAsync(token).ConfigureAwait(continueOnCapturedContext: false);
            try
            {
                if (_cache != null)
                {
                    return;
                }

                if (_options.ConnectionMultiplexerFactory == null)
                {
                    if (_options.ConfigurationOptions != null)
                    {
                        _connection = await ConnectionMultiplexer.ConnectAsync(_options.ConfigurationOptions).ConfigureAwait(continueOnCapturedContext: false);
                    }
                    else
                    {
                        _connection = await ConnectionMultiplexer.ConnectAsync(_options.Configuration).ConfigureAwait(continueOnCapturedContext: false);
                    }
                }
                else
                {
                    _connection = await _options.ConnectionMultiplexerFactory!().ConfigureAwait(continueOnCapturedContext: false);
                }

                PrepareConnection();
                _cache = _connection.GetDatabase();
            }
            finally
            {
                _connectionLock.Release();
            }
        }

        private void PrepareConnection()
        {
            ValidateServerFeatures();
            TryRegisterProfiler();
        }

        private void ValidateServerFeatures()
        {
            if (_connection == null)
            {
                throw new InvalidOperationException("_connection cannot be null.");
            }

            try
            {
                EndPoint[] endPoints = _connection.GetEndPoints();
                foreach (EndPoint endpoint in endPoints)
                {
                    if (_connection.GetServer(endpoint).Version < ServerVersionWithExtendedSetCommand)
                    {
                        _setScript = "\r\n                redis.call('HMSET', KEYS[1], 'absexp', ARGV[1], 'sldexp', ARGV[2], 'data', ARGV[4])\r\n                if ARGV[3] ~= '-1' then\r\n                  redis.call('EXPIRE', KEYS[1], ARGV[3])\r\n                end\r\n                return 1";
                        break;
                    }
                }
            }
            catch (NotSupportedException exception)
            {
                _setScript = "\r\n                redis.call('HMSET', KEYS[1], 'absexp', ARGV[1], 'sldexp', ARGV[2], 'data', ARGV[4])\r\n                if ARGV[3] ~= '-1' then\r\n                  redis.call('EXPIRE', KEYS[1], ARGV[3])\r\n                end\r\n                return 1";
            }
        }

        private void TryRegisterProfiler()
        {
            if (_connection == null)
            {
                throw new InvalidOperationException("_connection cannot be null.");
            }

            if (_options.ProfilingSession != null)
            {
                _connection.RegisterProfiler(_options.ProfilingSession);
            }
        }

        private void Refresh(IDatabase cache, string key, DateTimeOffset? absExpr, TimeSpan? sldExpr)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (sldExpr.HasValue)
            {
                TimeSpan? expiry;
                if (absExpr.HasValue)
                {
                    TimeSpan timeSpan = absExpr.Value - DateTimeOffset.Now;
                    expiry = ((timeSpan <= sldExpr.Value) ? new TimeSpan?(timeSpan) : sldExpr);
                }
                else
                {
                    expiry = sldExpr;
                }

                cache.KeyExpire(_instance + key, expiry, CommandFlags.None);
            }
        }

        private async Task RefreshAsync(IDatabase cache, string key, DateTimeOffset? absExpr, TimeSpan? sldExpr, CancellationToken token = default(CancellationToken))
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            token.ThrowIfCancellationRequested();
            if (sldExpr.HasValue)
            {
                TimeSpan? expiry;
                if (absExpr.HasValue)
                {
                    TimeSpan timeSpan = absExpr.Value - DateTimeOffset.Now;
                    expiry = ((timeSpan <= sldExpr.Value) ? new TimeSpan?(timeSpan) : sldExpr);
                }
                else
                {
                    expiry = sldExpr;
                }

                await cache.KeyExpireAsync(_instance + key, expiry, CommandFlags.None).ConfigureAwait(continueOnCapturedContext: false);
            }
        }

        public Task Clear()
        {
            return this._redisCache.Clear();
        }

        public Task<bool> Exist(string key)
        {
            return this._redisCache.Exist(key);
        }

        public Task<TEntity?> Get<TEntity>(string key)
        {
            return this._redisCache.Get<TEntity>(key);
        }

        public byte[]? HashGet(string redisKey, string field)
        {
            Connect();
            return this._cache.HashGet(redisKey, field);
        }

        public T? HashGet<T>(string redisKey, string field)
        {
            Connect();
            var value = this._cache.HashGet(redisKey, field);
            if (value == RedisValue.Null)
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(value!);
        }

        public T? HashGetAll<T>(string redisKey)
        {
            throw new NotImplementedException();
        }

        public Task<T?> HashGetAllAsync<T>(string key)
        {
            throw new NotImplementedException();
        }

        public Task<T?> HashGetAsync<T>(string redisKey, string key)
        {
            return this._redisCache.HashGetAsync<T>(redisKey, key);
        }

        public async Task<byte[]?> HashGetAsync(string redisKey, string field, CancellationToken token = default)
        {
            await ConnectAsync().ConfigureAwait(false);
            return await this._cache.HashGetAsync(redisKey, field);
        }

        public async Task<T?> HashGetAsync<T>(string redisKey, string field, CancellationToken token = default)
        {
            await ConnectAsync().ConfigureAwait(false);
            var value = await this._cache.HashGetAsync(redisKey, field);
            if (value == RedisValue.Null)
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(value!);
        }

        public void HashRemove(string redisKey, string field)
        {
            Connect();
            this._cache.HashDelete(redisKey, field);
        }

        public async Task HashRemoveAsync(string redisKey, string field, CancellationToken token = default)
        {
            await ConnectAsync().ConfigureAwait(false);
            await this._cache.HashDeleteAsync(redisKey, field).ConfigureAwait(false);
        }

        public void HashSet(string redisKey, string field, byte[]? value, DistributedCacheEntryOptions options)
        {
            this.Connect();
            this._cache.HashSet(redisKey, field, value);
        }

        public void HashSetAll(string redisKey, object? value, DistributedCacheEntryOptions options)
        {
            throw new NotImplementedException();
        }

        public Task HashSetAllAsync(string key, object? value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HashSetAsync(string redisKey, string key, string value)
        {
            return this._redisCache.HashSetAsync(redisKey, key, value);
        }

        public Task<bool> HashSetAsync<T>(string redisKey, string key, T value)
        {
            return this._redisCache.HashSetAsync<T>(redisKey, key, value);
        }

        public Task HashSetAsync(string key, string field, byte[]? value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task ListClearAsync(string redisKey)
        {
            return this._redisCache.ListClearAsync(redisKey);
        }

        public Task<long> ListDelRangeAsync(string redisKey, string redisValue, long type = 0)
        {
            return this._redisCache.ListDelRangeAsync(redisKey, redisValue, type);
        }

        public Task<T?> ListLeftPopAsync<T>(string redisKey) where T : class
        {
            return this._redisCache.ListLeftPopAsync<T>(redisKey);
        }

        public Task<string?> ListLeftPopAsync(string redisKey)
        {
            return this._redisCache.ListLeftPopAsync(redisKey);
        }

        public Task<long> ListLeftPushAsync(string redisKey, string redisValue)
        {
            return this._redisCache.ListLeftPushAsync(redisKey, redisValue);
        }

        public Task<long> ListLengthAsync(string redisKey)
        {
            return this._redisCache.ListLengthAsync(redisKey);
        }

        public Task<RedisValue[]> ListRangeAsync(string redisKey)
        {
            return this._redisCache.ListRangeAsync(redisKey);
        }

        public Task<IEnumerable<string>> ListRangeAsync(string redisKey, int db = -1)
        {
            return this._redisCache.ListRangeAsync(redisKey, db);
        }

        public Task<IEnumerable<string>> ListRangeAsync(string redisKey, int start, int stop)
        {
            return this._redisCache.ListRangeAsync(redisKey, start, stop);
        }

        public Task<T?> ListRightPopAsync<T>(string redisKey) where T : class
        {
            return this._redisCache.ListRightPopAsync<T>(redisKey);
        }

        public Task<string?> ListRightPopAsync(string redisKey)
        {
            return this._redisCache.ListRightPopAsync(redisKey);
        }

        public Task<long> ListRightPushAsync(string redisKey, string redisValue)
        {
            return this._redisCache.ListRightPushAsync(redisKey, redisValue);
        }

        public Task<long> ListRightPushAsync(string redisKey, IEnumerable<string> redisValue)
        {
            return this._redisCache.ListRightPushAsync(redisKey, redisValue);
        }

        public Task Set(string key, object value, TimeSpan cacheTime)
        {
            return this._redisCache.Set(key, value, cacheTime);
        }

        public Task SortedSetAddAsync(string redisKey, string redisValue, double score)
        {
            return this._redisCache.SortedSetAddAsync(redisKey, redisValue, score);
        }
        private void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }

    }
}
