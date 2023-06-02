using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ViazyNetCore.Handlers
{
    public class IocEventHandlerFactory : IEventHandlerFactory
    {
        public Type HandlerType { get; }

        protected IServiceScopeFactory ScopeFactory { get; }

        public IocEventHandlerFactory(IServiceScopeFactory scopeFactory, Type handlerType)
        {
            ScopeFactory = scopeFactory;
            HandlerType = handlerType;
        }

        public IEventHandlerDisposeWrapper GetHandler()
        {
            var scope = ScopeFactory.CreateScope();
            return new EventHandlerDisposeWrapper(
                (IEventHandler)scope.ServiceProvider.GetRequiredService(HandlerType),
                () => scope.Dispose()
            );
        }

        public bool IsInFactories(List<IEventHandlerFactory> handlerFactories)
        {
            return handlerFactories
                .OfType<IocEventHandlerFactory>()
                .Any(f => f.HandlerType == HandlerType);
        }
    }
}
