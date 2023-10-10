using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;
using IdentityServer4.Services;
using Microsoft.Extensions.Hosting;
using ViazyNetCore.IdentityService4.FreeSql;
using ViazyNetCore.IdentityService4.FreeSql.Options;
using ViazyNetCore.IdentityService4.FreeSql.Stores;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityServerFreeSqlBuilderExtensions
    {
        public static IIdentityServerBuilder AddConfigurationStore(
            this IIdentityServerBuilder builder
            , Action<ConfigurationStoreOptions> storeOptionsAction = null)
        {
            return builder.AddConfigurationStore<ConfigurationDbContext>(storeOptionsAction);
        }
        public static IIdentityServerBuilder AddConfigurationStore<TContext>(
            this IIdentityServerBuilder builder
            , Action<ConfigurationStoreOptions> storeOptionsAction = null)
            where TContext : DbContext, IConfigurationDbContext
        {
            builder.Services.AddConfigurationDbContext<TContext>(storeOptionsAction);

            builder.AddClientStore<ClientStore>();
            builder.AddResourceStore<ResourceStore>();
            //builder.AddCorsPolicyService<CorsPolicyService>();

            return builder;
        }
        public static IIdentityServerBuilder AddOperationalStore(this IIdentityServerBuilder builder
            , Action<OperationalStoreOptions> storeOptionsAction = null)
        {
            return builder.AddOperationalStore<PersistedGrantDbContext>(storeOptionsAction);
        }

        public static IIdentityServerBuilder AddOperationalStore<TContext>(this IIdentityServerBuilder builder
            , Action<OperationalStoreOptions> storeOptionsAction = null)
             where TContext : DbContext, IPersistedGrantDbContext
        {
            builder.Services.AddOperationalDbContext<TContext>(storeOptionsAction);

            builder.Services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();
            builder.Services.AddTransient<IDeviceFlowStore, DeviceFlowStore>();

            return builder;
        }

        public static IIdentityServerBuilder AddConfigurationStoreCache(this IIdentityServerBuilder builder)
        {
            // add the caching decorators
            builder.AddClientStoreCache<ClientStore>();
            builder.AddResourceStoreCache<ResourceStore>();
            builder.AddCorsPolicyCache<CorsPolicyService>();

            return builder;
        }

        public static IServiceCollection AddConfigurationDbContext(this IServiceCollection services
           , Action<ConfigurationStoreOptions> storeOptionsAction = null)
        {
            return services.AddConfigurationDbContext<ConfigurationDbContext>(storeOptionsAction);
        }

        public static IServiceCollection AddConfigurationDbContext<TContext>(this IServiceCollection services
            , Action<ConfigurationStoreOptions> storeOptionsAction = null)
            where TContext : DbContext, IConfigurationDbContext
        {
            var options = new ConfigurationStoreOptions();
            services.AddSingleton(options);
            storeOptionsAction?.Invoke(options);

            if(options.ResolveDbContextOptions != null)
            {
                services.AddFreeDbContext<TContext>(options: options.ConfigureDbContext);

            }
            else
            {
                services.AddFreeDbContext<TContext>(dbCtxBuilder =>
                {
                    options.ConfigureDbContext?.Invoke(dbCtxBuilder);
                });
            }
            services.AddScoped<IConfigurationDbContext, TContext>();

            return services;
        }

        public static IServiceCollection AddOperationalDbContext(this IServiceCollection services
           , Action<OperationalStoreOptions> storeOptionsAction = null)
        {
            return services.AddOperationalDbContext<PersistedGrantDbContext>(storeOptionsAction);
        }

        public static IServiceCollection AddOperationalDbContext<TContext>(this IServiceCollection services
            , Action<OperationalStoreOptions> storeOptionsAction = null)
            where TContext : DbContext, IPersistedGrantDbContext
        {
            var storeOptions = new OperationalStoreOptions();
            services.AddSingleton(storeOptions);
            storeOptionsAction?.Invoke(storeOptions);

            if(storeOptions.ResolveDbContextOptions != null)
            {
                services.AddFreeDbContext<TContext>(storeOptions.ConfigureDbContext);
            }
            else
            {
                services.AddFreeDbContext<TContext>(dbCtxBuilder =>
                {
                    storeOptions.ConfigureDbContext?.Invoke(dbCtxBuilder);
                });
            }

            services.AddScoped<IPersistedGrantDbContext, TContext>();
            //services.AddTransient<TokenCleanupService>();

            return services;
        }
    }
}
