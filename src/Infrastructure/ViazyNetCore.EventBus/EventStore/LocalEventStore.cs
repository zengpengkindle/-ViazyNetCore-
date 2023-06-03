using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using ViazyNetCore.EventBus;
using ViazyNetCore.Handlers;

namespace ViazyNetCore
{
    public class LocalEventStore : EventStoreBase
    {
        protected ConcurrentDictionary<Type, List<IEventHandlerFactory>> HandlerFactories { get; }
        public LocalEventStore(IServiceScopeFactory serviceScopeFactory, IEventHandlerInvoker eventHandlerInvoker)
            : base(serviceScopeFactory, eventHandlerInvoker)
        {
            this.HandlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
        }

        public override IEnumerable<Type> GetHandlersForEvent<T>()
        {
            return GetHandlersForEvent(typeof(T));
        }

        public override IEnumerable<Type> GetHandlersForEvent(Type eventType)
        {
            return GetOrCreateHandlerFactories(eventType).Select(p => p.GetHandler().GetType());
        }

        public override bool HasSubscribeForEvent<T>()
        {
            return HasSubscribeForEvent(typeof(T));
        }

        public override bool HasSubscribeForEvent(Type eventType)
        {
            return GetOrCreateHandlerFactories(eventType).Any();
        }

        public override void Unsubscribe<T, TH>()
        {
            var handlerToRemove = FindSubscribeToRemove(typeof(T), typeof(TH));
            if (handlerToRemove == null) return;
            this.Unsubscribe(typeof(T), handlerToRemove);
        }

        public override void Unsubscribe(Type eventType, IEventHandler eventHandler)
        {
            var handlerToRemove = FindSubscribeToRemove(eventType, eventHandler.GetType());
            if (handlerToRemove == null) return;
            this.Unsubscribe(eventType, handlerToRemove);
        }

        public override IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
        {
            GetOrCreateHandlerFactories(eventType)
                .Locking(factories =>
                {
                    if (!factory.IsInFactories(factories))
                    {
                        factories.Add(factory);
                    }
                }
                );

            return new EventHandlerFactoryUnregistrar(this, eventType, factory);
        }

        public override void Unsubscribe(Type eventType, IEventHandlerFactory factory)
        {
            GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Remove(factory));
        }

        public override void Unsubscribe<T>(Func<T, Task> action)
        {
            var actionHandler = new ActionEventHandlerAsync<T>(action);
            var handlerToRemove = FindSubscribeToRemove(typeof(T), actionHandler.GetType());
            if (handlerToRemove == null) return;
            this.Unsubscribe(typeof(T), handlerToRemove);
        }

        private IEventHandlerFactory? FindSubscribeToRemove(Type eventType, Type eventHandlerType)
        {
            if (!HasSubscribeForEvent(eventType))
            {
                return null;
            }
            return this.GetOrCreateHandlerFactories(eventType).FirstOrDefault(eh => eh.GetHandler().EventHandler.GetType() == eventHandlerType);
        }

        private List<IEventHandlerFactory> GetOrCreateHandlerFactories(Type eventType)
        {
            return HandlerFactories.GetOrAdd(eventType, (type) => new List<IEventHandlerFactory>());
        }

        public override IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType)
        {
            var handlerFactoryList = new List<EventTypeWithEventHandlerFactories>();

            foreach (var handlerFactory in HandlerFactories.Where(hf => ShouldTriggerEventForHandler(eventType, hf.Key)))
            {
                handlerFactoryList.Add(new EventTypeWithEventHandlerFactories(handlerFactory.Key, handlerFactory.Value));
            }

            return handlerFactoryList.ToArray();
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

        public override async Task PublishToEventBusAsync(Type baseEventType, object eventData)
        {
            await TriggerHandlersAsync(baseEventType, eventData);
        }
    }
}
