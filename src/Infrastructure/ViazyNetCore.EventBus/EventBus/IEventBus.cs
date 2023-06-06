using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore
{
    public interface IEventBus
    {
        /// <summary>
        /// 发布<see cref="IEventData"/>消息。
        /// </summary>
        /// <typeparam name="TEventData"></typeparam>
        /// <param name="event"></param>
        void Publish<TEventData>(TEventData eventData) where TEventData : IEventData;
        void Publish(Type eventType, object eventData);

        Task PublishAsync<TEventData>(TEventData eventData) where TEventData : IEventData;
        Task PublishAsync(Type eventType, object eventData);

    }
}
