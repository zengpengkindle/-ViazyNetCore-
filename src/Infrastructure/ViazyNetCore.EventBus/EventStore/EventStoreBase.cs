using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ViazyNetCore.Handlers;

namespace ViazyNetCore
{
    public abstract class EventStoreBase : IEventStore
    {
        public bool IsEmpty => false;

        public IServiceScopeFactory ServiceScopeFactory { get; }

        public EventStoreBase(IServiceScopeFactory serviceScopeFactory)
        {
            this.ServiceScopeFactory = serviceScopeFactory;
        }

        public void AddActionRegister<TEvent>(Func<TEvent, Task> action) where TEvent : IEventData
        {
            AddRegister(typeof(TEvent), new ActionEventHandlerAsync<TEvent>(action));
        }

        public void AddRegister<T, TH>()
            where T : IEventData
            where TH : IEventHandler, new()
        {
            Subscribe(typeof(T), new TransientEventHandlerFactory<TH>());
        }

        public void AddRegister(Type eventData, IEventHandler eventHandler)
        {
            Subscribe(eventData, new SingleInstanceHandlerFactory(eventHandler));
        }

        public void Clear()
        {
        }

        public abstract IEnumerable<Type> GetHandlersForEvent<T>() where T : IEventData;

        public abstract IEnumerable<Type> GetHandlersForEvent(Type eventData);

        public abstract bool HasRegisterForEvent<T>() where T : IEventData;

        public abstract bool HasRegisterForEvent(Type eventData);

        public void RegisterAllEventHandlerFromAssembly(Assembly assembly)
        {
            foreach (var type in assembly.DefinedTypes)
            {
                if (typeof(IEventHandler).IsAssignableFrom(type))//判断当前类型是否实现了IEventHandler接口
                {
                    var handlerAsyncInterface = type.GetInterface("IEventHandlerAsync`1");//获取该类实现的泛型接口
                    if (handlerAsyncInterface != null)
                    {
                        var eventAsycnDataType = handlerAsyncInterface.GetGenericArguments()[0]; // 获取泛型接口指定的参数类型

                        this.Subscribe(eventAsycnDataType, new IocEventHandlerFactory(ServiceScopeFactory, eventAsycnDataType!));
                    }
                }
            }
        }

        public void RegisterAllEventModulsFromAssembly(Assembly assembly)
        {
            foreach (var type in assembly.DefinedTypes)
            {
                if (typeof(IEventModuls).IsAssignableFrom(type))//判断当前类型是否实现了IEventHandler接口
                {
                    this.RegisterModule((IEventModuls)type);
                }
            }
        }

        public void RegisterModule(IEventModuls eventModuls)
        {
            eventModuls.RegisterEventHandler(this);
        }

        public abstract void RemoveRegister<T, TH>()
            where T : IEventData
            where TH : IEventHandler, new();

        public abstract void RemoveRegister(Type eventData, IEventHandler eventHandler);

        public abstract IDisposable Subscribe(Type eventType, IEventHandlerFactory factory);

        public abstract void Unsubscribe(Type eventType, IEventHandlerFactory factory);

        public abstract void RemoveActionRegister<T>(Func<T, Task> action) where T : IEventData;
    }
}
