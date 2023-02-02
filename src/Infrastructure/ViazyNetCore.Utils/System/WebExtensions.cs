using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace System
{
    public static class WebExtensions
    {

        private const string SCHEME_DELIMITER = "://";
        /// <summary>
        /// 获取当前请求的主机。
        /// </summary>
        /// <param name="request">请求。</param>
        /// <returns>主机地址。</returns>
        public static string GetHost(this HttpRequest request)
        {
            var host = request.Host.Value;
            var secheme = request.Scheme;
            if(request.Headers.TryGetValue("X-Http-scheme", out var v))
            {
                secheme = v.ToString();
            }

            var length = secheme.Length + SCHEME_DELIMITER.Length + host.Length;

            return new StringBuilder(length)
                .Append(secheme)
                .Append(SCHEME_DELIMITER)
                .Append(host)
                .ToString();
        }

        /// <summary>
        /// 获取当前上下文的主机。
        /// </summary>
        /// <param name="context">HTTP 上下文。</param>
        /// <returns>主机地址。</returns>
        public static string GetHost(this HttpContext context) => context.Request.GetHost();

        //private static IEnumerable<string> SplitCsv(string csvList)
        //{
        //    if(string.IsNullOrWhiteSpace(csvList)) return new string[0];
        //    return csvList
        //        .TrimEnd(',')
        //        .Split(',')
        //        .AsEnumerable()
        //        .Select(s => s.Trim());
        //}

        private static T GetHeaderValueAs<T>(string headerName, HttpContext httpContext)
        {
            if(httpContext?.Request?.Headers?.TryGetValue(headerName, out var values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if(!rawValues.IsNull())
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default;
        }

        private static readonly string KEY_IP = Guid.NewGuid().ToString("N");

        /// <summary>
        /// 获取当前上下文的客户端 IP 地址。
        /// </summary>
        /// <param name="context">HTTP 上下文。</param>
        /// <returns>IP 地址。</returns>
        public static string GetRequestIP(this HttpContext context)
        {
            return context.Temp(KEY_IP, () =>
            {
                // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

                // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
                // for 99% of cases however it has been suggested that a better (although tedious)
                // approach might be to read each IP from right to left and use the first public IP.
                // http://stackoverflow.com/a/43554000/538763
                //
                var ip = GetHeaderValueAs<string>("X-Real-IP", context)
                ?? GetHeaderValueAs<string>("X_SHOPIFY_CLIENT_IP", context)
                ?? GetHeaderValueAs<string>("CF_CONNECTING_IP", context)
                ?? GetHeaderValueAs<string>("CLIENT_IP", context)
                ?? GetHeaderValueAs<string>("X_FORWARDED_FOR", context)
                ;
                if(ip.IsNull())
                {
                    ip = GetHeaderValueAs<string>("X-Forwarded-For", context);

                    // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
                    // use this you must In project.json add a dependency to: "Microsoft.AspNetCore.HttpOverrides": "1.0.0"
                    // In Startup.cs, in the Configure() method add:
                    //  app.UseForwardedHeaders(new ForwardedHeadersOptions
                    //      {
                    //          ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                    //          ForwardedHeaders.XForwardedProto
                    //      }); 
                    if(ip.IsNull() && context?.Connection?.RemoteIpAddress != null)
                        ip = context.Connection.RemoteIpAddress.ToString();

                    if(ip.IsNull())
                        ip = GetHeaderValueAs<string>("REMOTE_ADDR", context);
                }
                // _httpContextAccessor.HttpContext?.Request?.Host this is the local host.
                if(ip == "::1")
                {
                    ip = "127.0.0.1";
                }

                return ip;
            });
        }

        /// <summary>
        /// 获取上下文的临时值。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="context">HTTP 上下文。</param>
        /// <param name="key">键名。</param>
        /// <param name="valueCreateFactory">默认值创建工厂。</param>
        /// <returns>值。</returns>
        public static T Temp<T>(this HttpContext context, string key, Func<T> valueCreateFactory = null)
        {
            var items = context.Items;
            var value = items[key];
            if(value is null)
            {
                if(valueCreateFactory is null) return default;
                var tValue = valueCreateFactory();
                context.Items[key] = tValue;
                return tValue;
            }
            return (T)value;
        }

        /// <summary>
        /// 设置上下文的临时值。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="context">HTTP 上下文。</param>
        /// <param name="key">键名。</param>
        /// <param name="value">值。</param>
        public static void Temp<T>(this HttpContext context, string key, T value)
        {
            var items = context.Items;
            if(EqualityComparer<T>.Default.Equals(value, default)) items.Remove(key);
            else items[key] = value;
        }

        /// <summary>
        /// 获取上下文的 COOKIE 值。
        /// </summary>
        /// <param name="context">HTTP 上下文。</param>
        /// <param name="key">键名。</param>
        /// <returns>值。</returns>
        public static string Cookie(this HttpContext context, string key)
        {
            return context.Request.Cookies.TryGetValue(key, out var value) ? value : string.Empty;
        }

        /// <summary>
        /// 设置上下文的 COOKIE 值。
        /// </summary>
        /// <param name="context">HTTP 上下文。</param>
        /// <param name="key">键名。</param>
        /// <param name="value">值。</param>
        /// <param name="options">COOKIE 配置。</param>
        public static void Cookie(this HttpContext context, string key, string value, CookieOptions options = null)
        {
            var cookies = context.Response.Cookies;

            if(value.IsNull())
            {
                cookies.Delete(key);
            }
            else
            {
                if(options != null)
                {
                    //cookies.Delete(key, options);
                    cookies.Append(key, value, options);
                }
                else
                {
                    // cookies.Delete(key);
                    cookies.Append(key, value);
                }
            }
        }

    }
}
