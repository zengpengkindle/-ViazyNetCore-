using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace ViazyNetCore.Redis
{
    public class DefaultRedisCache : RedisCache
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

        protected virtual DateTimeOffset? GetAbsoluteExpiration(
            DateTimeOffset creationTime,
            DistributedCacheEntryOptions options)
        {
            return (DateTimeOffset?)GetAbsoluteExpirationMethod.Invoke(null, new object[] { creationTime, options });
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
    }
}
