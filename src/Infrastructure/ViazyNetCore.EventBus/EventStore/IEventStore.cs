using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore
{
    public interface IEventStore
    {
        void AddRegister<T, TH>() where T : IEventData where TH : IEventHandler;
        void AddRegister(Type eventData, Type eventHandler);
        void AddActionRegister<T>(Action<T> action) where T : IEventData;

        bool TryAddRegister<T, TH>() where T : IEventData where TH : IEventHandler;
        bool TryAddRegister(Type eventData, Type eventHandler);
        bool TryAddActionRegister<T>(Action<T> action) where T : IEventData;

        void RemoveRegister<T, TH>() where T : IEventData where TH : IEventHandler;
        void RemoveActionRegister<T>(Action<T> action) where T : IEventData;
        void RemoveRegister(Type eventData, Type eventHandler);

        //bool TryRemoveRegister<T, TH>() where T : IEventData where TH : IEventHandler;
        //bool TryRemoveActionRegister<T>(Action<T> action) where T : IEventData;
        //bool TryRemoveRegister(Type eventData, Type eventHandler);

        bool HasRegisterForEvent<T>() where T : IEventData;
        bool HasRegisterForEvent(Type eventData);
        IEnumerable<Type> GetHandlersForEvent<T>() where T : IEventData;
        IEnumerable<Type> GetHandlersForEvent(Type eventData);

        Type GetEventTypeByName(string eventName);
        bool IsEmpty { get; }
        void Clear();

        void RegisterAllEventHandlerFromAssembly(Assembly assembly);

        void RegisterModule(IEventModuls eventModuls);

        void RegisterAllEventModulsFromAssembly(Assembly assembly);
    }
}
