using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using ViazyNetCore.Handlers;

namespace ViazyNetCore
{
    public class EventBus : IEventBus
    {
        private readonly IEventStore _eventStore;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public EventBus(IEventStore eventStore, IServiceScopeFactory serviceScopeFactory)
        {
            this._eventStore = eventStore;
            this._serviceScopeFactory = serviceScopeFactory;
        }

        public void Publish<TEventData>(TEventData @event) where TEventData : IEventData
        {
            this.PublishAsync(@event).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public void Publish<TEventData>(Type eventHandlerType, TEventData @event) where TEventData : IEventData
        {
            this.PublishAsync(eventHandlerType, @event).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task PublishAsync<TEventData>(TEventData @event) where TEventData : IEventData
        {
            if (@event is null) throw new ArgumentNullException(nameof(@event));
            //if(@event.EventSource is null) throw new ArgumentNullException(nameof(@event.EventSource));

            if (this._eventStore.HasRegisterForEvent<TEventData>())
            {
                var eventHanlderTypes = this._eventStore.GetHandlersForEvent<TEventData>().ToList();

                foreach (var handlerType in eventHanlderTypes)
                {
                    ////同步方法
                    //this.TriggerHandler(handlerType, @event);

                    await this.TriggerHandlerAsync(handlerType, @event);
                };
            }

        }

        public async Task PublishAsync<TEventData>(Type eventHandlerType, TEventData @event) where TEventData : IEventData
        {
            if (@event is null) throw new ArgumentNullException(nameof(@event));
            //if(@event.EventSource is null) throw new ArgumentNullException(nameof(@event.EventSource));

            if (this._eventStore.HasRegisterForEvent<TEventData>())
            {
                var eventHanlderTypes = this._eventStore.GetHandlersForEvent<TEventData>().ToList();

                //触发指定EventHandler
                if (eventHandlerType != null) eventHanlderTypes = eventHanlderTypes.Where(o => o == eventHandlerType).ToList();

                foreach (var handlerType in eventHanlderTypes)
                {
                    await this.TriggerHandlerAsync(handlerType, @event);
                };
            }

        }

        private async Task TriggerHandlerAsync<TEventData>(Type eventHandlerType, TEventData @event) where TEventData : IEventData
        {
            //异步方法
            var handlerAsyncInterface = eventHandlerType.GetInterface("IEventHandlerAsync`1");
            if (handlerAsyncInterface != null)
            {
                //从Ioc容器中获取所有的实例
                using var scope = this._serviceScopeFactory.CreateScope();
                var eventAsyncHandlers = scope.ServiceProvider.GetServices(handlerAsyncInterface);

                //循环遍历，仅当解析的实例类型与映射字典中事件处理类型一致时，才触发事件
                foreach (var eventAsyncHandler in eventAsyncHandlers)
                {
                    if (eventAsyncHandler!.GetType() == eventHandlerType)
                    {
                        if (eventAsyncHandler is IEventHandlerAsync<TEventData> handler)
                            await handler.HandleEventAsync(@event);
                    }
                }
            }

        }

    }
}
