using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Mvc.Filters
{
    public static class ActionContextExtensions
    {
        public static T GetRequiredService<T>(this FilterContext context)
            where T : class
        {
            return context.HttpContext.RequestServices.GetRequiredService<T>();
        }

        public static T GetService<T>(this FilterContext context, T defaultValue = default)
            where T : class
        {
            return context.HttpContext.RequestServices.GetService<T>() ?? defaultValue;
        }
    }

}
