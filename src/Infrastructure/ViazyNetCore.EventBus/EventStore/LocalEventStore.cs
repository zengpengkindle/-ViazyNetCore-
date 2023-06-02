using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ViazyNetCore.Handlers;

namespace ViazyNetCore
{
    public class LocalEventStore : EventStoreBase
    {
        protected ConcurrentDictionary<Type, List<IEventHandlerFactory>> HandlerFactories { get; }
        public LocalEventStore(IEventStore eventStore, IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
            this.HandlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
        }

        protected IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType)
        {
            var handlerFactoryList = new List<EventTypeWithEventHandlerFactories>();

            foreach (var handlerFactory in HandlerFactories.Where(hf => ShouldTriggerEventForHandler(eventType, hf.Key)))
            {
                handlerFactoryList.Add(new EventTypeWithEventHandlerFactories(handlerFactory.Key, handlerFactory.Value));
            }

            return handlerFactoryList.ToArray();
        }

        private List<IEventHandlerFactory> GetOrCreateHandlerFactories(Type eventType)
        {
            return HandlerFactories.GetOrAdd(eventType, (type) => new List<IEventHandlerFactory>());
        }

        private static bool ShouldTriggerEventForHandler(Type targetEventType, Type handlerEventType)
        {
            //Should trigger same type
            if (handlerEventType == targetEventType)
            {
                return true;
            }

            //Should trigger for inherited types
            if (handlerEventType.IsAssignableFrom(targetEventType))
            {
                return true;
            }

            return false;
        }

        public override IEnumerable<Type> GetHandlersForEvent<T>()
        {
            return GetHandlersForEvent(typeof(T));
        }

        public override IEnumerable<Type> GetHandlersForEvent(Type eventData)
        {
            return GetOrCreateHandlerFactories(eventData).Select(p => p.GetHandler().GetType());
        }

        public override bool HasRegisterForEvent<T>()
        {
            throw new NotImplementedException();
        }

        public override bool HasRegisterForEvent(Type eventData)
        {
            throw new NotImplementedException();
        }

        public override void RemoveRegister<T, TH>()
        {
            throw new NotImplementedException();
        }

        public override void RemoveRegister(Type eventData, IEventHandler eventHandler)
        {
            throw new NotImplementedException();
        }

        public override IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
        {
            throw new NotImplementedException();
        }

        public override void Unsubscribe(Type eventType, IEventHandlerFactory factory)
        {
            throw new NotImplementedException();
        }

        public override void RemoveActionRegister<T>(Func<T, Task> action)
        {
            throw new NotImplementedException();
        }

        #region Inner Class
        protected class EventTypeWithEventHandlerFactories
        {
            public Type EventType { get; }

            public List<IEventHandlerFactory> EventHandlerFactories { get; }

            public EventTypeWithEventHandlerFactories(Type eventType, List<IEventHandlerFactory> eventHandlerFactories)
            {
                EventType = eventType;
                EventHandlerFactories = eventHandlerFactories;
            }
        }
        #endregion
    }
}
