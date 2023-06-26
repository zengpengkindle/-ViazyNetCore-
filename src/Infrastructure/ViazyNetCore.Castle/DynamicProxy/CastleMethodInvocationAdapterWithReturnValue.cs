using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using ViazyNetCore.DynamicProxy;

namespace ViazyNetCore.Castle.DynamicProxy;

public class CastleMethodInvocationAdapterWithReturnValue<TResult> : CastleMethodInvocationAdapterBase, IMethodInvocation
{
    protected IInvocationProceedInfo ProceedInfo { get; }
    protected Func<IInvocation, IInvocationProceedInfo, Task<TResult>> Proceed { get; }

    public CastleMethodInvocationAdapterWithReturnValue(IInvocation invocation,
        IInvocationProceedInfo proceedInfo,
        Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
        : base(invocation)
    {
        ProceedInfo = proceedInfo;
        Proceed = proceed;
    }

    public override async Task ProceedAsync()
    {
        ReturnValue = await Proceed(Invocation, ProceedInfo);
    }
}
