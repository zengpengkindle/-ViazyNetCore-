using System;

namespace ViazyNetCore.Formatter.Response.Filters
{
    public class AutoWrapIgnoreAttribute : Attribute
    {
        public bool ShouldLogRequestData{ get; set; } = true;

        public AutoWrapIgnoreAttribute(){}
    }
}
