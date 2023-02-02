using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace ViazyNetCore
{
    public class EventBus : IEventBus
    {
        private readonly IEventStore _eventStore;
        private readonly IServiceProvider _serviceProvider;

        public EventBus(IEventStore eventStore, IServiceProvider serviceProvider)
        {
            this._eventStore = eventStore;
            this._serviceProvider = serviceProvider;
        }

        public void Publish<TEventData>(TEventData @event) where TEventData : IEventData
        {
            if(@event is null) throw new ArgumentNullException(nameof(@event));
            //if(@event.EventSource is null) throw new ArgumentNullException(nameof(@event.EventSource));

            if(this._eventStore.HasRegisterForEvent<TEventData>())
            {
                var eventHanlderTypes = this._eventStore.GetHandlersForEvent<TEventData>().ToList();

                foreach(var handlerType in eventHanlderTypes)
                {
                    //同步方法
                    this.HandleEvent(handlerType, @event);

                    //异步方法
                    this.HandleEventAsync(handlerType, @event).GetAwaiter().GetResult();
                };
            }
        }

        public void Publish<TEventData>(Type eventHandlerType, TEventData @event) where TEventData : IEventData
        {
            if(@event is null) throw new ArgumentNullException(nameof(@event));
            //if(@event.EventSource is null) throw new ArgumentNullException(nameof(@event.EventSource));

            if(this._eventStore.HasRegisterForEvent<TEventData>())
            {
                var eventHanlderTypes = this._eventStore.GetHandlersForEvent<TEventData>().ToList();

                //触发指定EventHandler
                if(eventHandlerType != null) eventHanlderTypes = eventHanlderTypes.Where(o => o == eventHandlerType).ToList();

                foreach(var handlerType in eventHanlderTypes)
                {
                    //同步方法
                    this.HandleEvent(handlerType, @event);

                    //异步方法
                    this.HandleEventAsync(handlerType, @event).GetAwaiter().GetResult();
                };
            }
        }

        public async Task PublishAsync<TEventData>(TEventData @event) where TEventData : IEventData
        {
            if(@event is null) throw new ArgumentNullException(nameof(@event));
            //if(@event.EventSource is null) throw new ArgumentNullException(nameof(@event.EventSource));

            if(this._eventStore.HasRegisterForEvent<TEventData>())
            {
                var eventHanlderTypes = this._eventStore.GetHandlersForEvent<TEventData>().ToList();

                foreach(var handlerType in eventHanlderTypes)
                {
                    //同步方法
                    this.HandleEvent(handlerType, @event);

                    await this.HandleEventAsync(handlerType, @event);
                };
            }

        }

        public async Task PublishAsync<TEventData>(Type eventHandlerType, TEventData @event) where TEventData : IEventData
        {
            if(@event is null) throw new ArgumentNullException(nameof(@event));
            //if(@event.EventSource is null) throw new ArgumentNullException(nameof(@event.EventSource));

            if(this._eventStore.HasRegisterForEvent<TEventData>())
            {
                var eventHanlderTypes = this._eventStore.GetHandlersForEvent<TEventData>().ToList();

                //触发指定EventHandler
                if(eventHandlerType != null) eventHanlderTypes = eventHanlderTypes.Where(o => o == eventHandlerType).ToList();

                foreach(var handlerType in eventHanlderTypes)
                {  //同步方法
                    this.HandleEvent(handlerType, @event);

                    await this.HandleEventAsync(handlerType, @event);
                };
            }

        }

        private void HandleEvent<TEventData>(Type eventHandlerType, TEventData @event) where TEventData : IEventData
        {
            //获取类型实现的泛型接口
            var handlerInterface = eventHandlerType.GetInterface("IEventHandler`1");
            if(handlerInterface != null)
            {
                //从Ioc容器中获取所有的实例
                var eventHandlers = this._serviceProvider.GetServices(handlerInterface);

                //循环遍历，仅当解析的实例类型与映射字典中事件处理类型一致时，才触发事件
                foreach(var eventHandler in eventHandlers)
                {
                    if(eventHandler!.GetType() == eventHandlerType)
                    {
                        var handler = eventHandler as IEventHandler<TEventData>;
                        handler?.HandleEvent(@event);
                    }
                }
            }
        }

        private async Task HandleEventAsync<TEventData>(Type eventHandlerType, TEventData @event) where TEventData : IEventData
        {
            //异步方法
            var handlerAsyncInterface = eventHandlerType.GetInterface("IEventHandlerAsync`1");
            if(handlerAsyncInterface != null)
            {
                //从Ioc容器中获取所有的实例
                var eventAsyncHandlers = this._serviceProvider.GetServices(handlerAsyncInterface);

                //循环遍历，仅当解析的实例类型与映射字典中事件处理类型一致时，才触发事件
                foreach(var eventAsyncHandler in eventAsyncHandlers)
                {
                    if(eventAsyncHandler!.GetType() == eventHandlerType)
                    {
                        if(eventAsyncHandler is IEventHandlerAsync<TEventData> handler)
                            await handler.HandleEventAsync(@event);
                    }
                }
            }

        }

    }
}
