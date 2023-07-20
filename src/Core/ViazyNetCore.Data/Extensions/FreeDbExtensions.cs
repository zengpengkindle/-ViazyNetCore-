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
            var dbConfig = configuration.Get<DbConfig>();

            services.Configure<DbConfig>(configuration);

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

        public static IApplicationBuilder UseFreeSql(this IApplicationBuilder app)
        {
            var fsql = app.ApplicationServices.GetService<IFreeSql>();
            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            var dbOption = app.ApplicationServices.GetService<IOptions<DbConfig>>();
            User user = new User(httpContextAccessor);
            if (fsql == null)
                throw new ArgumentNullException(nameof(IFreeSql));
            if (dbOption == null)
                throw new ArgumentNullException(nameof(DbConfig));
            //fsql.Aop.CurdAfter += Aop_CurdAfter; ;

            if (dbOption.Value.Tenant)
            {
                fsql.GlobalFilter.ApplyOnly<ITenant>(FilterNames.Tenant, a => a.TenantId == user.TenantId);
            }

            //会员过滤器
            fsql.GlobalFilter.ApplyOnlyIf<IMember>(FilterNames.Member,
                () =>
                {
                    if (user?.Id > 0 && user.IdentityType != AuthUserType.Member)
                        return false;
                    return true;
                },
                a => a.MemberId == user.Id
            );
            fsql.Aop.AuditValue += (s, e) =>
            {
                FreeSqlExtensions.AopAuditValue(user, e);
            };
            return app;
        }
    }
}
