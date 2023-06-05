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

        public void Publish<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            this.PublishAsync(eventData).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public void Publish<TEventData>(Type eventHandlerType, TEventData eventData) where TEventData : IEventData
        {
            this.PublishAsync(eventHandlerType, eventData).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task PublishAsync<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            if (eventData is null) throw new ArgumentNullException(nameof(eventData));
            await this.PublishAsync(typeof(TEventData), eventData);
        }

        public virtual async Task PublishAsync<TEventData>(Type eventHandlerType, TEventData eventData) where TEventData : IEventData
        {
            if (eventData is null) throw new ArgumentNullException(nameof(eventData));
            await this._localEventStore.PublishToEventBusAsync(eventHandlerType, eventData);
        }
    }
}
