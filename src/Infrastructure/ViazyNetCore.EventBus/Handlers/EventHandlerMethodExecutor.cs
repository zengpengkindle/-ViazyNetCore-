using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore
{

    public delegate Task EventHandlerMethodExecutorAsync(IEventHandler target, object parameter);

    public interface IEventHandlerMethodExecutor
    {
        EventHandlerMethodExecutorAsync ExecutorAsync { get; }
    }

    public class LocalEventHandlerMethodExecutor<TEvent> : IEventHandlerMethodExecutor
        where TEvent : IEventData
    {
        public EventHandlerMethodExecutorAsync ExecutorAsync => (target, parameter) => target.As<IEventHandlerAsync<TEvent>>().HandleEventAsync((TEvent)parameter);

        public Task ExecuteAsync(IEventHandler target, TEvent parameters)
        {
            return ExecutorAsync(target, parameters);
        }
    }

    public class DistributedEventHandlerMethodExecutor<TEvent> : IEventHandlerMethodExecutor
        where TEvent : IEventData
    {
        public EventHandlerMethodExecutorAsync ExecutorAsync => (target, parameter) => target.As<IEventHandlerAsync<TEvent>>().HandleEventAsync((TEvent)parameter);

        public Task ExecuteAsync(IEventHandler target, TEvent parameters)
        {
            return ExecutorAsync(target, parameters);
        }
    }

}
