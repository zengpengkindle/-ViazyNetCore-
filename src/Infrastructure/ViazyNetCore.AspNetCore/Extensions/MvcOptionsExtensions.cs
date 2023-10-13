using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ViazyNetCore.AspNetCore.Mvc.ExceptionFilters;
using ViazyNetCore.AspNetCore.Mvc.Filterrs;

namespace ViazyNetCore.AspNetCore.Extensions
{
    internal static class MvcOptionsExtensions
    {
        public static void AddMvcDefault(this MvcOptions options, IServiceCollection services)
        {
            AddActionFilters(options);
        }

        private static void AddActionFilters(MvcOptions options)
        {
            options.Filters.AddService<AuditLogActionFilter>();
            options.Filters.AddService<ValidationActionFilter>();
            options.Filters.AddService<ResponseWrapActionFilter>();
            options.Filters.AddService<ControllerExceptionFilter>();
        }
    }
}
