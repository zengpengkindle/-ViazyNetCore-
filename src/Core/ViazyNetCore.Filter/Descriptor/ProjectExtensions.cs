using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using HttpMethod = Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpMethod;
using Microsoft.OpenApi.Exceptions;

namespace ViazyNetCore.APIs
{
    /// <summary>
    /// 项目扩展方法类
    /// </summary>
    public static class ProjectExtensions
    {
        /// <summary>
        /// 获取HTTP请求方法。
        /// </summary>
        /// <param name="info">当前方法实例。</param>
        /// <returns>返回HTTP请求方法。</returns>
        public static string GetHttpMethod(this MemberInfo info)
        {
            if (info.IsDefined(typeof(HttpPostAttribute)))
                return HttpMethod.Post.ToString();
            if (info.IsDefined(typeof(HttpPutAttribute)))
                return HttpMethod.Put.ToString();
            if (info.IsDefined(typeof(HttpDeleteAttribute)))
                return HttpMethod.Delete.ToString();
            if (info.IsDefined(typeof(HttpHeadAttribute)))
                return HttpMethod.Head.ToString();
            if (info.IsDefined(typeof(HttpPatchAttribute)))
                return HttpMethod.Patch.ToString();
            if (info.IsDefined(typeof(HttpOptionsAttribute)))
                return HttpMethod.Options.ToString();
            return HttpMethod.Get.ToString();
        }

        /// <summary>
        /// API接口备注扩展
        /// </summary>
        /// <param name="service">服务</param>
        /// <param name="action">配置</param>
        /// <returns></returns>
        public static IServiceCollection AddApiDescriptor(this IServiceCollection service, Action<ApiDescriptorOptions> action)
        {
            var options = new ApiDescriptorOptions();
            action.Invoke(options);
            if (string.IsNullOrEmpty(options.CachePrefix))
                throw new OpenApiException("Cache prefix is ​​required");
            if (string.IsNullOrEmpty(options.ServiceName))
                throw new OpenApiException("Service name is required");

            service.AddSingleton(options);
            //service.Configure(action);
            if (service.All(c => c.ServiceType != typeof(IMemoryCache)))
                service.AddMemoryCache();
            service.AddSingleton<IApiManager, ApiManager>();
            var apiManager = service.BuildServiceProvider().GetRequiredService<IApiManager>();
            var apiDescriptors = apiManager.GetApiDescriptors();
            service.AddSingleton(apiDescriptors);

            return service;
        }
    }
}
