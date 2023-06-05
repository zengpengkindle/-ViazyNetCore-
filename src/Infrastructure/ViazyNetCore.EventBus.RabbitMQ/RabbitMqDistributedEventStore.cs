using System.Collections.Concurrent;
using System.MQueue;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ViazyNetCore.EventBus.Distributed;
using ViazyNetCore.Handlers;

namespace ViazyNetCore.EventBus.RabbitMQ
{
    public class RabbitMqDistributedEventStore : DistributedEventStoreBase
    {
        private readonly IMessageBus _messageBus;

        //protected RabbitMqEventBusOptions RabbitMqEventBusOptions { get; }
        protected ConcurrentDictionary<Type, List<IEventHandlerFactory>> HandlerFactories { get; }
        protected ConcurrentDictionary<string, Type> EventTypes { get; }

        public RabbitMqDistributedEventStore(IServiceScopeFactory serviceScopeFactory, IEventHandlerInvoker eventHandlerInvoker
            //, IOptions<RabbitMqEventBusOptions> options
            , IMessageBus messageBus)
            : base(serviceScopeFactory, eventHandlerInvoker)
        {
            //this.RabbitMqEventBusOptions = options.Value;
            this._messageBus = messageBus;

            this.HandlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
            this.EventTypes = new ConcurrentDictionary<string, Type>();
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

        public override IEnumerable<Type> GetHandlersForEvent<T>()
        {
            return GetHandlersForEvent(typeof(T));
        }

        public override IEnumerable<Type> GetHandlersForEvent(Type eventType)
        {
            return GetOrCreateHandlerFactories(eventType).Select(p => p.HandlerType);
        }

        public override bool HasSubscribeForEvent<T>()
        {
            return HasSubscribeForEvent(typeof(T));
        }

        public override bool HasSubscribeForEvent(Type eventType)
        {
            return GetOrCreateHandlerFactories(eventType).Any();
        }

        public override async Task PublishToEventBusAsync(Type eventType, object eventData)
        {
            using var context = _messageBus.Context;
            await context.PublishAsync(eventType, eventData);
        }

        public override IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
        {
            var handlerFactories = GetOrCreateHandlerFactories(eventType);
            if (factory.IsInFactories(handlerFactories))
            {
                return NullDisposable.Instance;
            }
            handlerFactories.Add(factory);

            //if (handlerFactories.Count == 1) //TODO: Multi-threading!
            //{
            //    Consumer.BindAsync(EventNameAttribute.GetNameOrDefault(eventType));
            //}

            return new EventHandlerFactoryUnregistrar(this, eventType, factory);
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

        protected override byte[] Serialize(object eventData)
        {
            return JSON.Serialize(eventData);
        }
        private List<IEventHandlerFactory> GetOrCreateHandlerFactories(Type eventType)
        {
            return HandlerFactories.GetOrAdd(
                eventType,
                type =>
                {
                    var eventName = EventNameAttribute.GetNameOrDefault(type);
                    EventTypes[eventName] = type;
                    return new List<IEventHandlerFactory>();
                }
            );
        }
    }
}