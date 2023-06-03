using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ViazyNetCore.EventBus;
using ViazyNetCore.EventBus.Distributed;
using ViazyNetCore.Handlers;

namespace ViazyNetCore
{
    public abstract class EventStoreBase : IEventStore
    {

        public IServiceScopeFactory ServiceScopeFactory { get; }
        public IEventHandlerInvoker EventHandlerInvoker { get; }

        public EventStoreBase(IServiceScopeFactory serviceScopeFactory, IEventHandlerInvoker eventHandlerInvoker)
        {
            this.ServiceScopeFactory = serviceScopeFactory;
            this.EventHandlerInvoker = eventHandlerInvoker;
        }

        public void Subscribe<TEvent>(Func<TEvent, Task> action) where TEvent : IEventData
        {
            Subscribe(typeof(TEvent), new ActionEventHandlerAsync<TEvent>(action));
        }

        public void Subscribe<T, TH>()
            where T : IEventData
            where TH : IEventHandler, new()
        {
            Subscribe(typeof(T), new TransientEventHandlerFactory<TH>());
        }

        public void Subscribe(Type eventData, IEventHandler eventHandler)
        {
            Subscribe(eventData, new SingleInstanceHandlerFactory(eventHandler));
        }

        public abstract IEnumerable<Type> GetHandlersForEvent<T>() where T : IEventData;

        public abstract IEnumerable<Type> GetHandlersForEvent(Type eventType);

        public abstract bool HasSubscribeForEvent<T>() where T : IEventData;

        public abstract bool HasSubscribeForEvent(Type eventType);

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

                        this.Subscribe(eventAsycnDataType, new IocEventHandlerFactory(ServiceScopeFactory, type));
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

        public abstract void Unsubscribe<T, TH>()
            where T : IEventData
            where TH : IEventHandler, new();

        public abstract void Unsubscribe(Type eventType, IEventHandler eventHandler);

        public abstract IDisposable Subscribe(Type eventType, IEventHandlerFactory factory);

        public abstract void Unsubscribe(Type eventType, IEventHandlerFactory factory);

        public abstract void Unsubscribe<T>(Func<T, Task> action) where T : IEventData;

        public abstract IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType);

        public abstract Task PublishToEventBusAsync(Type baseEventType, object eventData);

        public virtual async Task TriggerHandlersAsync(Type eventType, object eventData)
        {
            var exceptions = new List<Exception>();

            await TriggerHandlersAsync(eventType, eventData, exceptions);

            if (exceptions.Any())
            {
                ThrowOriginalExceptions(eventType, exceptions);
            }
        }

        protected virtual async Task TriggerHandlersAsync(Type eventType, object eventData, List<Exception> exceptions, InboxConfig inboxConfig = null)
        {
            await new SynchronizationContextRemover();

            foreach (var handlerFactories in this.GetHandlerFactories(eventType))
            {
                foreach (var handlerFactory in handlerFactories.EventHandlerFactories)
                {
                    await TriggerHandlerAsync(handlerFactory, handlerFactories.EventType, eventData, exceptions, inboxConfig);
                }
            }

            //Implements generic argument inheritance. See IEventDataWithInheritableGenericArgument
            if (eventType.GetTypeInfo().IsGenericType &&
                eventType.GetGenericArguments().Length == 1 &&
                typeof(IEventData).IsAssignableFrom(eventType))
            {
                var genericArg = eventType.GetGenericArguments()[0];
                var baseArg = genericArg.GetTypeInfo().BaseType;
                if (baseArg != null)
                {
                    var baseEventType = eventType.GetGenericTypeDefinition().MakeGenericType(baseArg);
                    //var constructorArgs = ((IEventData)eventData).GetConstructorArgs();
                    //var baseEventData = Activator.CreateInstance(baseEventType, constructorArgs);
                    await this.PublishToEventBusAsync(baseEventType, eventData);
                }
            }
        }


        protected virtual async Task TriggerHandlerAsync(IEventHandlerFactory asyncHandlerFactory, Type eventType,
            object eventData, List<Exception> exceptions, InboxConfig inboxConfig = null)
        {
            using (var eventHandlerWrapper = asyncHandlerFactory.GetHandler())
            {
                try
                {
                    var handlerType = eventHandlerWrapper.EventHandler.GetType();

                    if (inboxConfig?.HandlerSelector != null &&
                        !inboxConfig.HandlerSelector(handlerType))
                    {
                        return;
                    }

                    await EventHandlerInvoker.InvokeAsync(eventHandlerWrapper.EventHandler, eventData, eventType);
                }
                catch (TargetInvocationException ex)
                {
                    exceptions.Add(ex.InnerException);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }
        }

        protected void ThrowOriginalExceptions(Type eventType, List<Exception> exceptions)
        {
            if (exceptions.Count == 1)
            {
                exceptions[0].ReThrow();
            }

            throw new AggregateException(
                "More than one error has occurred while triggering the event: " + eventType,
                exceptions
            );
        }
        #region InnerClass
        protected struct SynchronizationContextRemover : INotifyCompletion
        {
            public bool IsCompleted
            {
                get { return SynchronizationContext.Current == null; }
            }

            public void OnCompleted(Action continuation)
            {
                var prevContext = SynchronizationContext.Current;
                try
                {
                    SynchronizationContext.SetSynchronizationContext(null);
                    continuation();
                }
                finally
                {
                    SynchronizationContext.SetSynchronizationContext(prevContext);
                }
            }

            public SynchronizationContextRemover GetAwaiter()
            {
                return this;
            }

            public void GetResult()
            {
            }
        }

        #endregion
    }
}
