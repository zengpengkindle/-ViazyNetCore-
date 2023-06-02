using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Handlers;

namespace ViazyNetCore
{
    public class SingleInstanceHandlerFactory : IEventHandlerFactory
    {
        /// <summary>
        /// The event handler instance.
        /// </summary>
        public IEventHandler HandlerInstance { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler"></param>
        public SingleInstanceHandlerFactory(IEventHandler handler)
        {
            HandlerInstance = handler;
        }

        public IEventHandlerDisposeWrapper GetHandler()
        {
            return new EventHandlerDisposeWrapper(HandlerInstance);
        }

        public bool IsInFactories(List<IEventHandlerFactory> handlerFactories)
        {
            return handlerFactories
                .OfType<SingleInstanceHandlerFactory>()
                .Any(f => f.HandlerInstance == HandlerInstance);
        }
    }
}
