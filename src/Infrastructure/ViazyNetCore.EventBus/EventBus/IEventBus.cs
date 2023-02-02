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
        void Publish<TEventData>(TEventData @event) where TEventData : IEventData;
        void Publish<TEventData>(Type eventHandlerType, TEventData @event) where TEventData : IEventData;

        Task PublishAsync<TEventData>(TEventData @event) where TEventData : IEventData;
        Task PublishAsync<TEventData>(Type eventHandlerType, TEventData @event) where TEventData : IEventData;

    }
}
