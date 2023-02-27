using System;

namespace ViazyNetCore.Formatter.Response.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequestDataLogIgnoreAttribute : Attribute
    {
    }
}
