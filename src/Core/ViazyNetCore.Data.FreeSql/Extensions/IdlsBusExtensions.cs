using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FreeSql;
using Microsoft.Extensions.DependencyInjection;
using ViazyNetCore.IdleBus;
using ViazyNetCore.Repository;

namespace ViazyNetCore
{
    public static class IdlsBusExtensions
    {
        static AsyncLocal<string> AsyncLocalTenantId = new();
        public static IdleBus<IFreeSql> ChangeTenant(this IdleBus<IFreeSql> ib, string tenantId)
        {
            AsyncLocalTenantId.Value = tenantId;
            return ib;
        }
        public static IFreeSql Get(this IdleBus<IFreeSql> ib) => ib.Get(AsyncLocalTenantId.Value ?? "default");
        public static IBaseRepository<T> GetRepository<T>(this IdleBus<IFreeSql> ib) where T : class => ib.Get().GetRepository<T>();

        public static IServiceCollection AddRepository(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(DefaultIdlsRepository<>));
            services.AddScoped(typeof(BaseRepository<>), typeof(DefaultIdlsRepository<>));

            services.AddScoped(typeof(IBaseRepository<,>), typeof(DefaultIdlsRepository<,>));
            services.AddScoped(typeof(BaseRepository<,>), typeof(DefaultIdlsRepository<,>));

            if (assemblies?.Any() == true)
                foreach (var asse in assemblies)
                    foreach (var repo in asse.GetTypes().Where(a => a.IsAbstract == false && typeof(IBaseRepository).IsAssignableFrom(a)))
                        services.AddScoped(repo);

            return services;
        }
    }
}
