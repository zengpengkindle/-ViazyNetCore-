using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace ViazyNetCore.Redis
{
    public interface IRedisPubCache
    {
        long Publish<T>(string channel, T msg);
        Task<long> PublishAsync<T>(string channel, T msg);
        void Subscribe(string subChannel, Action<RedisChannel, RedisValue> handler = null);
        Task SubscribeAsync(string subChannel, Action<RedisChannel, RedisValue> handler = null);
        void Unsubscribe(string channel);
        void UnsubscribeAll();
    }
}
