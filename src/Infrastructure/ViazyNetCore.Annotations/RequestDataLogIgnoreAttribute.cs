using System;

namespace ViazyNetCore
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequestDataLogIgnoreAttribute : Attribute
    {
    }
}
