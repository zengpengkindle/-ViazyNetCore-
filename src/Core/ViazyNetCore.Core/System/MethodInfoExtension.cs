using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class MethodInfoExtension
    {

        internal static Type GetReturnType(this MethodInfo method)
        {
            var isAsync = method.IsAsync();
            var returnType = method.ReturnType;
            return isAsync ? (returnType.GenericTypeArguments.FirstOrDefault() ?? typeof(void)) : returnType;
        }
    }
}
