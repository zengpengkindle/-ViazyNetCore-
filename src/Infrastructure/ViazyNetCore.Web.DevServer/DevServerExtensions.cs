using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.DevServer;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 表示 Node 开发服务器的扩展。
    /// </summary>
    public static class DevServerExtensions
    {
        private const string DEFAULT_PATH = "/index.html";

        internal static bool IsSpaRequest(this HttpContext httpContext)
        {
            return Path.GetExtension(httpContext.Request.Path).Length == 0;
        }

        /// <summary>
        /// 应用 Node 开发服务器。
        /// </summary>
        /// <param name="spaBuilder">一个 SPA 生成器。</param>
        /// <param name="options">配置。</param>
        public static void UseDevServer(this ISpaBuilder spaBuilder, NodeServerOptionsBase options)
        {
            if (spaBuilder is null)
            {
                throw new ArgumentNullException(nameof(spaBuilder));
            }

            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var spaOptions = spaBuilder.Options;

            if (string.IsNullOrEmpty(spaOptions.SourcePath))
            {
                throw new InvalidOperationException(
                    $"To use {nameof(UseDevServer)}, you must supply a non-empty value for the {nameof(SpaOptions.SourcePath)} property of {nameof(SpaOptions)} when calling {nameof(SpaApplicationBuilderExtensions.UseSpa)}.");
            }

            DevServerMiddleware.Attach(spaBuilder, options);
        }

        /// <summary>
        /// 启用基于 History 模式。
        /// </summary>
        /// <param name="appBuilder">应用程序生成器。</param>
        /// <param name="defaultPath">默认首页路径。</param>
        /// <returns>应用程序生成器。</returns>
        public static IApplicationBuilder UseHistoryFallback(this IApplicationBuilder appBuilder, PathString defaultPath
            = default)
        {
            if (!defaultPath.HasValue) defaultPath = DEFAULT_PATH;
            //- 所有 GET 请求并且没有后缀名的请求默认为 SPA 模式
            return appBuilder.Use((context, next) =>
            {
                if (HttpMethods.IsGet(context.Request.Method)&&!context.Request.Path.StartsWithSegments("swagger") && context.IsSpaRequest())
                {
                    context.Request.Path = defaultPath;
                }
                return next();
            });
        }
    }
}
