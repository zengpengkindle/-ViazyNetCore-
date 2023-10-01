using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ViazyNetCore;
using ViazyNetCore.Data.FreeSql;
using ViazyNetCore.Data.FreeSql.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class FreeDbExtensions
    {
        public static void AddFreeDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IUser, User>();
            services.Configure<DbConfig>(configuration);
            //var serviceProvider = services.BuildServiceProvider();

            services.AddSingleton(serviceProvider =>
            {
                var user = serviceProvider.GetService<IUser>();
                var dbConfig = serviceProvider.GetService<IOptions<DbConfig>>()!.Value;

                var freeSqlCloud = new FreeSqlCloud<string>();
                FreeSqlExtensions.RegisterDb(freeSqlCloud, dbConfig, user);
                if (dbConfig.Dbs?.Length > 0)
                {
                    foreach (var db in dbConfig.Dbs)
                    {
                        FreeSqlExtensions.RegisterDb(freeSqlCloud, db, user);
                    }
                }
                var fsql = freeSqlCloud.Use(dbConfig.Key);

                return freeSqlCloud;
            });
            services.AddSingleton(serviceProvider =>
            {
                var freeSqlCloud = serviceProvider.GetService<FreeSqlCloud<string>>()!;
                var dbConfig = serviceProvider.GetService<IOptions<DbConfig>>()!.Value;
                var fsql = freeSqlCloud.Use(dbConfig.Key);
                return fsql;
            });

            services.AddScoped<UnitOfWorkManagerCloud>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(GuidRepository<>));
            services.AddScoped(typeof(BaseRepository<>), typeof(GuidRepository<>));

            services.AddScoped(typeof(IBaseRepository<,>), typeof(DefaultRepository<,>));
            services.AddScoped(typeof(BaseRepository<,>), typeof(DefaultRepository<,>));
        }

        public static IApplicationBuilder UseFreeSql(this IApplicationBuilder app)
        {
            var fsql = app.ApplicationServices.GetService<IFreeSql>();
            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            var user = app.ApplicationServices.GetService<IUser>();
            var dbOption = app.ApplicationServices.GetService<IOptions<DbConfig>>();
            if (fsql == null)
                throw new ArgumentNullException(nameof(IFreeSql));
            if (dbOption == null)
                throw new ArgumentNullException(nameof(DbConfig));
            return app;
        }
    }
}
