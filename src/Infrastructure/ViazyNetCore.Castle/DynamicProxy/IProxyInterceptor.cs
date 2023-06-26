using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.DynamicProxy
{
    public interface IProxyInterceptor
    {
        Task InterceptAsync(IMethodInvocation invocation);
    }
}
