using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using ViazyNetCore;
using ViazyNetCore.AttachmentProvider;
using ViazyNetCore.AttachmentProvider.Hosts;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public class LocalStoreOption
    {
        /// <summary>
        /// 
        /// </summary>
        public string RequestPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string StoreRootPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<MediaType> MediaTypes { get; set; }
    }
}
/// <summary>
/// 
/// </summary>
public static class AttachmentStartup
{
    /// <summary>
    /// 添加本地附件存储服务。
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IServiceCollection AddLocalStoreProvider(this IServiceCollection services, Action<LocalStoreOption> options = null)
    {
        services.TryAddSingleton<IPathFormatter, DefaultPathFormatter>();
        services.TryAddSingleton<IStoreProvider>(sp =>
        {
            var storeProvider = sp.CreateInstance<StoreProvider>();
            var localStoreOption = new LocalStoreOption();
            if(options != null)
                options.Invoke(localStoreOption);

            if(localStoreOption.StoreRootPath is null)
                throw new NotImplementedException("StoreRootPath can't be Null");
            //
            var root = sp.GetRequiredService<IWebHostEnvironment>().ContentRootFileProvider.GetDirectoryInfo(localStoreOption.StoreRootPath, true);

            storeProvider.AddLocalHost(new LocalStoreOptions()
            {
                ServerUrl = localStoreOption.RequestPath,
                StoreRootPath = root.PhysicalPath,
                MediaTypes = localStoreOption.MediaTypes
            });

            return storeProvider;
        });

        return services;
    }

    /// <summary>
    /// 添加附件存储服务。
    /// </summary>
    /// <param name="services"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IServiceCollection AddStoreProvider(this IServiceCollection services, Action<IStoreProvider> action = null)
    {
        services.TryAddSingleton<IStoreProvider>(sp =>
        {
            var storeProvider = sp.CreateInstance<StoreProvider>();
            if(action != null)
                action.Invoke(storeProvider);
            else
            {
                var storeRootPath = sp.GetRequiredService<IWebHostEnvironment>().WebRootFileProvider.GetDirectoryInfo("").PhysicalPath;
                storeProvider.AddLocalHost(new LocalStoreOptions()
                {
                    ServerUrl = null,
                    StoreRootPath = storeRootPath
                });
            }
            return storeProvider;
        });

        return services;
    }

    /// <summary>
    /// 应用存储服务。
    /// </summary>
    /// <param name="app"></param>
    public static void UseStoreProvider(this IApplicationBuilder app)
    {
        var storeProvider = app.ApplicationServices.GetRequiredService<IStoreProvider>();
        if(storeProvider.LocalStoreHost != null)
        {
            var storeOptions = storeProvider.LocalStoreHost.Options as LocalStoreOptions;
            if(storeOptions.ServerUrl != null)
            {
                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(storeOptions.StoreRootPath),
                    RequestPath = storeOptions.ServerUrl
                });
            }
        }
    }
}

