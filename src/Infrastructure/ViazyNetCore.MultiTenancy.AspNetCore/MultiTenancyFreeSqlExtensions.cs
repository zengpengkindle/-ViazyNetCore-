using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FreeSql;
using FreeSql.Aop;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ViazyNetCore.Data.FreeSql;

namespace ViazyNetCore.MultiTenancy.AspNetCore
{
    public static class MultiTenancyFreeSqlExtensions
    {
        public static IApplicationBuilder UseMultiTenancyFreeSql(this IApplicationBuilder app)
        {
            var fsql = app.ApplicationServices.GetService<IFreeSql>();
            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            var user = app.ApplicationServices.GetRequiredService<IUser>();
            var currentTenant = app.ApplicationServices.GetService<ICurrentTenant>();
            var dbOption = app.ApplicationServices.GetService<IOptions<DbConfig>>();
            if (fsql == null)
                throw new ArgumentNullException(nameof(IFreeSql));
            if (dbOption == null)
                throw new ArgumentNullException(nameof(DbConfig));
            //fsql.Aop.CurdAfter += Aop_CurdAfter; ;

            // if (dbOption.Value.Tenant && currentTenant != null)
            {
                fsql.GlobalFilter.ApplyOnly<ITenant>(FilterNames.Tenant, a => a.TenantId == (currentTenant.Id ?? 0));
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
