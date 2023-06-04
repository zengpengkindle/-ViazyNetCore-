using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using ViazyNetCore.Handlers;
using ViazyNetCore.EventBus.Distributed;
using System.Runtime.CompilerServices;

namespace ViazyNetCore
{
    public class DefaultEventBus : IEventBus
    {
        private readonly IEventStore _localEventStore;

        public IEventHandlerInvoker EventHandlerInvoker { get; }

        public DefaultEventBus(ILocalEventStore localEventStore
            , IEventHandlerInvoker eventHandlerInvoker)
        {
            this._localEventStore = localEventStore;
            this.EventHandlerInvoker = eventHandlerInvoker;
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
            await this.PublishAsync(typeof(TEventData), @event);
        }

        public virtual async Task PublishAsync<TEventData>(Type eventHandlerType, TEventData @event) where TEventData : IEventData
        {
            if (@event is null) throw new ArgumentNullException(nameof(@event));
            await this._localEventStore.PublishToEventBusAsync(eventHandlerType, @event);
        }
    }
}
