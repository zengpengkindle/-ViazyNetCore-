using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ViazyNetCore.Handlers;

namespace ViazyNetCore.EventBus.Distributed
{
    public abstract class DistributedEventStoreBase : EventStoreBase, IDistributedEventStore
    {
        public DistributedEventStoreBase(IServiceScopeFactory serviceScopeFactory, IEventHandlerInvoker eventHandlerInvoker)
            : base(serviceScopeFactory, eventHandlerInvoker)
        {
        }
        public IDisposable Subscribe<TEvent>(IDistributedEventHandler<TEvent> handler) where TEvent : IEventData
        {
            return Subscribe(typeof(TEvent), handler);
        }

        public Task PublishAsync<TEvent>(TEvent eventData, bool onUnitOfWorkComplete = true, bool useOutbox = true) where TEvent : IEventData
        {
            return PublishAsync(typeof(TEvent), eventData, onUnitOfWorkComplete, useOutbox);
        }
        public async Task PublishAsync(Type eventType, object eventData, bool onUnitOfWorkComplete = true, bool useOutbox = true)
        {
            //if (useOutbox)
            //{
            //    if (await AddToOutboxAsync(eventType, eventData))
            //    {
            //        return;
            //    }
            //}
            await PublishToEventBusAsync(eventType, eventData);
        }

        //private async Task<bool> AddToOutboxAsync(Type eventType, object eventData)
        //{
        //    foreach (var outboxConfig in DistributedEventBusOptions.Outboxes.Values.OrderBy(x => x.Selector is null))
        //    {
        //        if (outboxConfig.Selector == null || outboxConfig.Selector(eventType))
        //        {
        //            using var scope = ServiceScopeFactory.CreateScope();
        //            var eventOutbox =
        //                (IEventOutbox)scope.GetRequiredService(outboxConfig.ImplementationType);
        //            var eventName = EventNameAttribute.GetNameOrDefault(eventType);
        //            await eventOutbox.EnqueueAsync(
        //                    eventName,
        //                    Serialize(eventData)
        //                );
        //            return true;
        //        }
        //    }

        //    return false;
        //}
        protected abstract byte[] Serialize(object eventData);
    }
}
