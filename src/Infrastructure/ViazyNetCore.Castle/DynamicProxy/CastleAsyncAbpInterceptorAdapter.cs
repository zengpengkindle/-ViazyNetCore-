using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using ViazyNetCore.DynamicProxy;

namespace ViazyNetCore.Castle.DynamicProxy;

public class CastleAsyncAbpInterceptorAdapter<TInterceptor> : AsyncInterceptorBase
    where TInterceptor : IProxyInterceptor
{
    private readonly TInterceptor _interceptor;

    public CastleAsyncAbpInterceptorAdapter(TInterceptor interceptor)
    {
        _interceptor = interceptor;
    }

    protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
    {
        await _interceptor.InterceptAsync(
            new CastleMethodInvocationAdapter(invocation, proceedInfo, proceed)
        );
    }

    protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
    {
        var adapter = new CastleMethodInvocationAdapterWithReturnValue<TResult>(invocation, proceedInfo, proceed);

        await _interceptor.InterceptAsync(
            adapter
        );

        return (TResult)adapter.ReturnValue;
    }
}
