using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.EventBus.Distributed
{
    public interface IDistributedEventStore : IEventStore
    {
        /// <summary>
        /// Registers to an event. 
        /// Same (given) instance of the handler is used for all event occurrences.
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="handler">Object to handle the event</param>
        IDisposable Subscribe<TEvent>(IDistributedEventHandler<TEvent> handler)
            where TEvent : IEventData;

        Task PublishAsync<TEvent>(
            TEvent eventData,
            bool onUnitOfWorkComplete = true,
            bool useOutbox = true)
            where TEvent : IEventData;

        Task PublishAsync(
            Type eventType,
            object eventData,
            bool onUnitOfWorkComplete = true,
            bool useOutbox = true);
    }
}
