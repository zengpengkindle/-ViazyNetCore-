﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore
{
    public class EventHandlerInvoker : IEventHandlerInvoker
    {
        private readonly ConcurrentDictionary<string, EventHandlerInvokerCacheItem> _cache;

        public EventHandlerInvoker()
        {
            _cache = new ConcurrentDictionary<string, EventHandlerInvokerCacheItem>();
        }

        public async Task InvokeAsync(IEventHandler eventHandler, object eventData, Type eventType)
        {
            var cacheItem = _cache.GetOrAdd($"{eventHandler.GetType().FullName}-{eventType.FullName}", _ =>
            {
                var item = new EventHandlerInvokerCacheItem();

                if (typeof(ILocalEventHandler<>).MakeGenericType(eventType).IsInstanceOfType(eventHandler))
                {
                    item.Local = (IEventHandlerMethodExecutor)Activator.CreateInstance(typeof(LocalEventHandlerMethodExecutor<>).MakeGenericType(eventType));
                }

                if (typeof(IDistributedEventHandler<>).MakeGenericType(eventType).IsInstanceOfType(eventHandler))
                {
                    item.Distributed = (IEventHandlerMethodExecutor)Activator.CreateInstance(typeof(DistributedEventHandlerMethodExecutor<>).MakeGenericType(eventType));
                }

                return item;
            });

            if (cacheItem.Local != null)
            {
                await cacheItem.Local.ExecutorAsync(eventHandler, eventData);
            }

            if (cacheItem.Distributed != null)
            {
                await cacheItem.Distributed.ExecutorAsync(eventHandler, eventData);
            }

            if (cacheItem.Local == null && cacheItem.Distributed == null)
            {
                throw new Exception("The object instance is not an event handler. Object type: " + eventHandler.GetType().AssemblyQualifiedName);
            }
        }
    }

    public class EventHandlerInvokerCacheItem
    {
        public IEventHandlerMethodExecutor Local { get; set; }

        public IEventHandlerMethodExecutor Distributed { get; set; }
    }
}
