using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;
using Microsoft.Extensions.Configuration;
using ViazyNetCore.Data.FreeSql;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class FreeDbExtensions
    {
        public static void AddFreeDb(this IServiceCollection services, IConfiguration configuration)
        {
            var dbConfig = configuration.Get<DbConfig>();
            var freeSqlCloud = new FreeSqlCloud<string>();
            FreeSqlExtensions.RegisterDb(freeSqlCloud, dbConfig);
            if (dbConfig.Dbs?.Length > 0)
            {
                foreach (var db in dbConfig.Dbs)
                {
                    FreeSqlExtensions.RegisterDb(freeSqlCloud, db);
                }
            }
            services.AddSingleton<IFreeSql>(freeSqlCloud);
            services.AddSingleton(freeSqlCloud);
            services.AddScoped<UnitOfWorkManagerCloud>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(GuidRepository<>));
            services.AddScoped(typeof(BaseRepository<>), typeof(GuidRepository<>));

            services.AddScoped(typeof(IBaseRepository<,>), typeof(DefaultRepository<,>));
            services.AddScoped(typeof(BaseRepository<,>), typeof(DefaultRepository<,>));

            var fsql = freeSqlCloud.Use(dbConfig.Key);
            services.AddSingleton(provider => fsql);
        }
    }
}
