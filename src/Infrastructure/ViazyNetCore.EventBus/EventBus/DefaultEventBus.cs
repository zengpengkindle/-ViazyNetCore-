using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using ViazyNetCore.Handlers;
using ViazyNetCore.EventBus.Distributed;
using System.Runtime.CompilerServices;

namespace ViazyNetCore
{
    public class DefaultEventBus : IEventBus
    {
        private readonly IEventStore _eventStore;
        private readonly IServiceProvider _serviceProvider;

        public IEventHandlerInvoker EventHandlerInvoker { get; }

        public DefaultEventBus(ILocalEventStore localEventStore
            , IServiceProvider serviceProvider
            , IEventHandlerInvoker eventHandlerInvoker)
        {
            this._eventStore = localEventStore;
            this._serviceProvider = serviceProvider;
            this.EventHandlerInvoker = eventHandlerInvoker;
        }

        public void Publish<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            this.PublishAsync(eventData).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public void Publish(Type eventType, object eventData)
        {
            this.PublishAsync(eventType, eventData).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task PublishAsync<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            if (eventData is null) throw new ArgumentNullException(nameof(eventData));
            await this.PublishAsync(typeof(TEventData), eventData);
        }

        public virtual async Task PublishAsync(Type eventType, object eventData)
        {
            if (eventData is null) throw new ArgumentNullException(nameof(eventData));
            //await this._eventStore.PublishToEventBusAsync(eventType, eventData, priority);

            var eventHanlderTypes = this._eventStore.GetHandlersForEvent(eventType).ToList();
            foreach (var handlerType in eventHanlderTypes)
            {
                //同步方法
                await this.HandleEventAsync(handlerType, eventData);
            };
        }

        private async Task HandleEventAsync(Type eventHandlerType, object @event)
        {
            var eventDataType = @event.GetType();
            //异步方法
            var handlerAsyncInterface = eventHandlerType.GetInterface("ILocalEventHandler`1");
            if (handlerAsyncInterface != null)
            {
                //从Ioc容器中获取所有的实例
                var eventAsyncHandlers = this._serviceProvider.GetServices(handlerAsyncInterface);

                //循环遍历，仅当解析的实例类型与映射字典中事件处理类型一致时，才触发事件
                foreach (var eventAsyncHandler in eventAsyncHandlers)
                {
                    if (eventAsyncHandler!.GetType() == eventHandlerType)
                    {
                        if (typeof(ILocalEventHandler<>).MakeGenericType(eventDataType).IsInstanceOfType(eventAsyncHandler))
                        {
                            var executor = (IEventHandlerMethodExecutor)Activator.CreateInstance(typeof(LocalEventHandlerMethodExecutor<>).MakeGenericType(eventDataType));
                            if (eventAsyncHandler is IEventHandler handler)
                                await executor.ExecutorAsync(handler, @event);
                        }
                    }
                }
            }

        }
    }
}
