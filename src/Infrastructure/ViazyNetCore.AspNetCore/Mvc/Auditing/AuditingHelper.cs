using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace ViazyNetCore.AspNetCore.Mvc.Auditing
{
    [Injection(Lifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient)]
    public class AuditingHelper : IAuditingHelper
    {
        protected IUser CurrentUser { get; }
        protected IServiceProvider ServiceProvider { get; }
        protected AppOptions Options { get; }

        public AuditingHelper(IUser currentUser
            , IOptions<AppOptions> options
            , IServiceProvider serviceProvider)
        {
            this.CurrentUser = currentUser;
            this.ServiceProvider = serviceProvider;
            this.Options = options.Value;
        }

        public virtual AuditLogInfo CreateAuditLogInfo()
        {
            var auditLogInfo = new AuditLogInfo()
            {
                ApplicationName = Options.ApplicationName,
                UserId = CurrentUser.Id,
                UserName = CurrentUser.Username,
                ExecutionTime = DateTime.Now
            };
            return auditLogInfo;
        }

        public virtual bool ShouldSaveAudit(MethodInfo methodInfo, bool defaultValue = false, bool ignoreIntegrationServiceAttribute = false)
        {
            if (methodInfo == null)
            {
                return false;
            }

            if (!methodInfo.IsPublic)
            {
                return false;
            }

            if (methodInfo.IsDefined(typeof(AutoWrapIgnoreAttribute)))
            {
                return false;
            }

            var classType = methodInfo.DeclaringType;
            if (classType != null)
            {
                if (classType.IsDefined(typeof(AutoWrapIgnoreAttribute), true))
                {
                    return false;
                }
            }

            return defaultValue;
        }
    }
}
