using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace ViazyNetCore.Redis
{
    public partial class RedisService
    {
        public ISubscriber GetSubscriber()
        {
            return this.GetRedisConnection().GetSubscriber();
        }

        public Task SubscribeAsync(string subChannel, Action<RedisChannel, RedisValue> handler = null)
        {
            var sub = this.GetSubscriber();
            return sub.SubscribeAsync(subChannel, (channel, message) =>
                  {
                      if (handler == null)
                      {
                          Console.WriteLine(subChannel + " 订阅收到消息：" + message);
                      }
                      else
                      {
                          handler(channel, message);
                      }
                  });
        }

        public void Subscribe(string subChannel, Action<RedisChannel, RedisValue> handler = null)
        {
            var sub = this.GetSubscriber();
            sub.Subscribe(subChannel, (channel, message) =>
               {
                   if (handler == null)
                   {
                       Console.WriteLine(subChannel + " 订阅收到消息：" + message);
                   }
                   else
                   {
                       handler(channel, message);
                   }
               });
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public long Publish<T>(string channel, T msg)
        {
            ISubscriber sub = this.GetSubscriber();
            return sub.Publish(channel, JSON.Stringify(msg));
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public Task<long> PublishAsync<T>(string channel, T msg)
        {
            ISubscriber sub = this.GetSubscriber();
            return sub.PublishAsync(channel, JSON.Stringify(msg));
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="channel"></param>
        public void Unsubscribe(string channel)
        {
            ISubscriber sub = this.GetSubscriber();
            sub.Unsubscribe(channel);
        }

        /// <summary>
        /// 取消全部订阅
        /// </summary>
        public void UnsubscribeAll()
        {
            ISubscriber sub = this.GetSubscriber();
            sub.UnsubscribeAll();
        }
    }
}
