using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using ViazyNetCore;
using ViazyNetCore.Modules;

namespace Microsoft.Extensions.DependencyInjection;

public static class WebApplicationBuilderExtensions
{
    public static ILogger<T> GetInitLogger<T>(this IServiceCollection services)
    {
        return services.GetSingletonInstance<ILogger<T>>();
    }

    public static async Task<IApplicationWithExternalServiceProvider> AddApplicationAsync<TStartupModule>(
       this WebApplicationBuilder builder,
       Action<ApplicationCreationOptions> optionsAction = null)
       where TStartupModule : IInjectionModule
    {
        return await builder.Services.AddApplicationAsync<TStartupModule>(options =>
        {
            options.Services.ReplaceConfiguration(builder.Configuration);
            optionsAction?.Invoke(options);
            if (options.Environment.IsNullOrWhiteSpace())
            {
                options.Environment = builder.Environment.EnvironmentName;
            }
        });
    }
    public async static Task<IApplicationWithExternalServiceProvider> AddApplicationAsync<TStartupModule>(this IServiceCollection services, Action<ApplicationCreationOptions> optionsAction = null)
        where TStartupModule : IInjectionModule
    {
        return await ApplicationFactory.CreateAsync<TStartupModule>(services, optionsAction);
    }
}
