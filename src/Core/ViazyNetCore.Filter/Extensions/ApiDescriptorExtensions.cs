using Microsoft.Extensions.Caching.Memory;
using Microsoft.OpenApi.Exceptions;
using ViazyNetCore.Filter;
using ViazyNetCore.Filter.Descriptor;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 项目扩展方法类
    /// </summary>
    public static class ApiDescriptorExtensions
    {
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

            if (service.All(c => c.ServiceType != typeof(IMemoryCache)))
                service.AddMemoryCache();
            service.AddSingleton<IApiManager, ApiManager>();
            //service.AddSingleton(sp =>
            //{
            //    var apiManager = sp.GetRequiredService<IApiManager>();
            //    var apiDescriptors = apiManager.GetApiDescriptors();
            //    return apiDescriptors;
            //});

            return service;
        }
    }
}
