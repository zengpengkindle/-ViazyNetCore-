using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using ViazyNetCore.Caching;

namespace ViazyNetCore.Redis
{
    public class DefaultRedisCache : RedisCache, IDistributedHashCache
    {
        protected static readonly string AbsoluteExpirationKey;
        protected static readonly string SlidingExpirationKey;
        protected static readonly string DataKey;
        protected static readonly long NotPresent;

        private readonly static FieldInfo SetScriptField;
        private readonly static FieldInfo RedisDatabaseField;
        private readonly static MethodInfo ConnectMethod;
        private readonly static MethodInfo ConnectAsyncMethod;
        private readonly static MethodInfo MapMetadataMethod;
        private readonly static MethodInfo GetAbsoluteExpirationMethod;
        private readonly static MethodInfo GetExpirationInSecondsMethod;

        protected IDatabase RedisDatabase => GetRedisDatabase();
        private IDatabase _redisDatabase;

        protected string Instance { get; }

        static DefaultRedisCache()
        {
            var type = typeof(RedisCache);

            RedisDatabaseField = Check.NotNull(type.GetField("_cache", BindingFlags.Instance | BindingFlags.NonPublic), nameof(RedisDatabaseField))!;

            SetScriptField = Check.NotNull(type.GetField("_setScript", BindingFlags.Instance | BindingFlags.NonPublic), nameof(SetScriptField))!;

            ConnectMethod = Check.NotNull(type.GetMethod("Connect", BindingFlags.Instance | BindingFlags.NonPublic), nameof(ConnectMethod))!;

            ConnectAsyncMethod = Check.NotNull(type.GetMethod("ConnectAsync", BindingFlags.Instance | BindingFlags.NonPublic), nameof(ConnectAsyncMethod))!;

            MapMetadataMethod = Check.NotNull(type.GetMethod("MapMetadata", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static), nameof(MapMetadataMethod))!;

            GetAbsoluteExpirationMethod = Check.NotNull(type.GetMethod("GetAbsoluteExpiration", BindingFlags.Static | BindingFlags.NonPublic), nameof(GetAbsoluteExpirationMethod))!;

            GetExpirationInSecondsMethod = Check.NotNull(type.GetMethod("GetExpirationInSeconds", BindingFlags.Static | BindingFlags.NonPublic), nameof(GetExpirationInSecondsMethod))!;

            AbsoluteExpirationKey = type.GetField("AbsoluteExpirationKey", BindingFlags.Static | BindingFlags.NonPublic)?.GetValue(null).ToString()!;

            SlidingExpirationKey = type.GetField("SlidingExpirationKey", BindingFlags.Static | BindingFlags.NonPublic)?.GetValue(null).ToString()!;

            DataKey = type.GetField("DataKey", BindingFlags.Static | BindingFlags.NonPublic)?.GetValue(null).ToString()!;

            // ReSharper disable once PossibleNullReferenceException
            NotPresent = Check.NotNull(type.GetField("NotPresent", BindingFlags.Static | BindingFlags.NonPublic), nameof(NotPresent))!.GetValue(null)!.To<int>();
        }
        public DefaultRedisCache(IOptions<RedisCacheOptions> optionsAccessor)
    : base(optionsAccessor)
        {
            Instance = optionsAccessor.Value.InstanceName ?? string.Empty;
        }

        protected virtual void Connect()
        {
            if (GetRedisDatabase() != null)
            {
                return;
            }

            ConnectMethod.Invoke(this, Array.Empty<object>());
        }

        protected virtual async Task ConnectAsync(CancellationToken token = default)
        {
            if (GetRedisDatabase() != null)
            {
                return;
            }

            await (Task)ConnectAsyncMethod.Invoke(this, new object[] { token });
        }


        private IDatabase GetRedisDatabase()
        {
            if (_redisDatabase == null)
            {
                _redisDatabase = RedisDatabaseField.GetValue(this) as IDatabase;
            }

            return _redisDatabase;
        }

        private string GetSetScript()
        {
            return SetScriptField?.GetValue(this).ToString();
        }

        public byte[]? HashGet(string key, string field)
        {
            Connect();
            return this._redisDatabase.HashGet(key, field);
        }

        public async Task<byte[]?> HashGetAsync(string key, string field, CancellationToken token = default)
        {
            await ConnectAsync().ConfigureAwait(false);
            return await this._redisDatabase.HashGetAsync(key, field);
        }

        public T? HashGet<T>(string key, string field)
        {
            Connect();
            var value = this._redisDatabase.HashGet(key, field);
            if (value == RedisValue.Null)
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(value!);
        }

        public async Task<T?> HashGetAsync<T>(string key, string field, CancellationToken token = default)
        {
            await ConnectAsync().ConfigureAwait(false);
            var value = await this._redisDatabase.HashGetAsync(key, field);
            if (value == RedisValue.Null)
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(value!);
        }

        public void HashRemove(string key, string field)
        {
            Connect();
            this._redisDatabase.HashDelete(key, field);
        }

        public async Task HashRemoveAsync(string key, string field, CancellationToken token = default)
        {
            await ConnectAsync().ConfigureAwait(false);
            await this._redisDatabase.HashDeleteAsync(key, field);
        }

        public void HashSet(string key, string field, byte[]? value, DistributedCacheEntryOptions options)
        {
            Connect();
            this._redisDatabase.HashSet(key, field, value);

            DateTimeOffset utcNow = DateTimeOffset.UtcNow;
            DateTimeOffset? absoluteExpiration = GetAbsoluteExpiration(utcNow, options);
            var expirationSeconds = GetExpirationInSeconds(utcNow, absoluteExpiration, options);
            this._redisDatabase.KeyExpire(key, expirationSeconds == null ? null : TimeSpan.FromSeconds(expirationSeconds.Value));
        }

        public async Task HashSetAsync(string key, string field, byte[]? value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            await ConnectAsync().ConfigureAwait(false);
            await this._redisDatabase.HashSetAsync(key, field, value);

            DateTimeOffset utcNow = DateTimeOffset.UtcNow;
            DateTimeOffset? absoluteExpiration = GetAbsoluteExpiration(utcNow, options);
            var expirationSeconds = GetExpirationInSeconds(utcNow, absoluteExpiration, options);
            await this._redisDatabase.KeyExpireAsync(key, expirationSeconds == null ? null : TimeSpan.FromSeconds(expirationSeconds.Value));
        }

        public void HashSetAll(string key, object? value, DistributedCacheEntryOptions options)
        {
            Check.NotNull(key, nameof(key));
            Check.NotNull(value, nameof(value));
            Check.NotNull(options, nameof(options));

            Dictionary<string, byte[]> dict;
            if (value is System.Collections.IDictionary d)
            {
                dict = new Dictionary<string, byte[]>(d.Count);
                foreach (System.Collections.DictionaryEntry item in d)
                {
                    dict.Add(Convert.ToString(item.Key).MustBe(), item.Value?.Object2Bytes() ?? Array.Empty<byte>());
                }
            }
            else
            {
                var typeMapper = TypeMapper.Create(value.GetType());
                dict = new Dictionary<string, byte[]>(typeMapper.Count);
                foreach (var propertyMapper in typeMapper.Properties)
                {
                    dict.Add(propertyMapper.Name, propertyMapper.GetValue(value).Object2Bytes());
                }
            }

            foreach (var item in dict)
            {
                this.HashSet(key, item.Key, item.Value, options);
            }
            DateTimeOffset utcNow = DateTimeOffset.UtcNow;
            DateTimeOffset? absoluteExpiration = GetAbsoluteExpiration(utcNow, options);
            var expirationSeconds = GetExpirationInSeconds(utcNow, absoluteExpiration, options);
            this._redisDatabase.KeyExpire(key, expirationSeconds == null ? null : TimeSpan.FromSeconds(expirationSeconds.Value));
        }

        public async Task HashSetAllAsync(string key, object? value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            Check.NotNull(key, nameof(key));
            Check.NotNull(value, nameof(value));
            Check.NotNull(options, nameof(options));

            Dictionary<string, byte[]> dict;
            if (value is System.Collections.IDictionary d)
            {
                dict = new Dictionary<string, byte[]>(d.Count);
                foreach (System.Collections.DictionaryEntry item in d)
                {
                    dict.Add(Convert.ToString(item.Key).MustBe(), item.Value?.Object2Bytes() ?? Array.Empty<byte>());
                }
            }
            else
            {
                var typeMapper = TypeMapper.Create(value.GetType());
                dict = new Dictionary<string, byte[]>(typeMapper.Count);
                foreach (var propertyMapper in typeMapper.Properties)
                {
                    dict.Add(propertyMapper.Name, propertyMapper.GetValue(value).Object2Bytes());
                }
            }

            foreach (var item in dict)
            {
                await this.HashSetAsync(key, item.Key, item.Value, options);
            }
            DateTimeOffset utcNow = DateTimeOffset.UtcNow;
            DateTimeOffset? absoluteExpiration = GetAbsoluteExpiration(utcNow, options);
            var expirationSeconds = GetExpirationInSeconds(utcNow, absoluteExpiration, options);
            await this._redisDatabase.KeyExpireAsync(key, expirationSeconds == null ? null : TimeSpan.FromSeconds(expirationSeconds.Value));
        }

        public T? HashGetAll<T>(string key)
        {
            Connect();
            var value = this._redisDatabase.StringGet(key);
            if (value == RedisValue.Null)
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(value!);
        }

        public async Task<T?> HashGetAllAsync<T>(string key)
        {
            await ConnectAsync().ConfigureAwait(false);
            var value = await this._redisDatabase.StringGetAsync(key);
            if (value == RedisValue.Null)
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(value!);
        }

        private static double? GetExpirationInSeconds(DateTimeOffset creationTime, DateTimeOffset? absoluteExpiration, DistributedCacheEntryOptions options)
        {
            if (absoluteExpiration.HasValue && options.SlidingExpiration.HasValue)
            {
                return (double)Math.Min((absoluteExpiration.Value - creationTime).TotalSeconds, options.SlidingExpiration.Value.TotalSeconds);
            }

            if (absoluteExpiration.HasValue)
            {
                return (double)(absoluteExpiration.Value - creationTime).TotalSeconds;
            }

            if (options.SlidingExpiration.HasValue)
            {
                return (double)options.SlidingExpiration.Value.TotalSeconds;
            }

            return null;
        }

        private static DateTimeOffset? GetAbsoluteExpiration(DateTimeOffset creationTime, DistributedCacheEntryOptions options)
        {
            if (options.AbsoluteExpiration.HasValue && options.AbsoluteExpiration <= creationTime)
            {
                throw new ArgumentOutOfRangeException("AbsoluteExpiration", options.AbsoluteExpiration.Value, "The absolute expiration value must be in the future.");
            }

            if (options.AbsoluteExpirationRelativeToNow.HasValue)
            {
                DateTimeOffset value = creationTime;
                TimeSpan? absoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow;
                return value + absoluteExpirationRelativeToNow;
            }

            return options.AbsoluteExpiration;
        }


        //protected virtual DateTimeOffset? GetAbsoluteExpiration(
        //    DateTimeOffset creationTime,
        //    DistributedCacheEntryOptions options)
        //{
        //    return (DateTimeOffset?)GetAbsoluteExpirationMethod.Invoke(null, new object[] { creationTime, options });
        //}
    }
}
