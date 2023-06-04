using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.EventBus;
using ViazyNetCore.Handlers;

namespace ViazyNetCore
{
    public interface IEventStore
    {
        IDisposable Subscribe(Type eventType, IEventHandlerFactory factory);

        IDisposable Subscribe<T, TH>() where T : IEventData where TH : IEventHandler, new();
        IDisposable Subscribe(Type eventType, IEventHandler eventHandler);
        IDisposable Subscribe<TEvent>(Func<TEvent, Task> action) where TEvent : IEventData;

        void Unsubscribe<T, TH>() where T : IEventData where TH : IEventHandler, new();
        void Unsubscribe<T>(Func<T, Task> action) where T : IEventData;
        void Unsubscribe(Type eventType, IEventHandler eventHandler);
        void Unsubscribe(Type eventType, IEventHandlerFactory factory);

        bool HasSubscribeForEvent<T>() where T : IEventData;
        bool HasSubscribeForEvent(Type eventData);

        IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType);
        IEnumerable<Type> GetHandlersForEvent<T>() where T : IEventData;
        IEnumerable<Type> GetHandlersForEvent(Type eventType);

        void RegisterAllEventHandlerFromAssembly(Assembly assembly);
        void RegisterModule(IEventModuls eventModuls);

        void RegisterAllEventModulsFromAssembly(Assembly assembly);
        Task PublishToEventBusAsync(Type baseEventType, object eventData);
    }
}
