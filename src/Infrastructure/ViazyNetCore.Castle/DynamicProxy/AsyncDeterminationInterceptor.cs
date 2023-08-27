using Castle.DynamicProxy;
using ViazyNetCore.DynamicProxy;

namespace ViazyNetCore.Castle.DynamicProxy;

public class AsyncDeterminationInterceptor<TInterceptor> : AsyncDeterminationInterceptor
    where TInterceptor : IProxyInterceptor
{
    public AsyncDeterminationInterceptor(TInterceptor interceptor)
        : base(new CastleAsyncAbpInterceptorAdapter<TInterceptor>(interceptor))
    {

    }
}
