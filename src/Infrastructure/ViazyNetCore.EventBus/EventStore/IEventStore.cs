using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Handlers;

namespace ViazyNetCore
{
    public interface IEventStore
    {
        void AddRegister<T, TH>() where T : IEventData where TH : IEventHandler, new();
        void AddRegister(Type eventData, IEventHandler eventHandler);
        void AddActionRegister<TEvent>(Func<TEvent, Task> action) where TEvent : IEventData;

        //bool TryAddRegister<T, TH>() where T : IEventData where TH : IEventHandler, new();
        //bool TryAddRegister(Type eventData, Type eventHandler);

        void RemoveRegister<T, TH>() where T : IEventData where TH : IEventHandler, new();
        void RemoveActionRegister<T>(Func<T, Task> action) where T : IEventData;
        void RemoveRegister(Type eventData, IEventHandler eventHandler);

        //bool TryRemoveRegister<T, TH>() where T : IEventData where TH : IEventHandler;
        //bool TryRemoveActionRegister<T>(Action<T> action) where T : IEventData;
        //bool TryRemoveRegister(Type eventData, Type eventHandler);

        bool HasRegisterForEvent<T>() where T : IEventData;
        bool HasRegisterForEvent(Type eventData);
        IEnumerable<Type> GetHandlersForEvent<T>() where T : IEventData;
        IEnumerable<Type> GetHandlersForEvent(Type eventData);

        bool IsEmpty { get; }
        void Clear();

        void RegisterAllEventHandlerFromAssembly(Assembly assembly);

        void RegisterModule(IEventModuls eventModuls);

        void RegisterAllEventModulsFromAssembly(Assembly assembly);
        void Unsubscribe(Type eventType, IEventHandlerFactory factory);
        IDisposable Subscribe(Type eventType, IEventHandlerFactory factory);
    }
}
