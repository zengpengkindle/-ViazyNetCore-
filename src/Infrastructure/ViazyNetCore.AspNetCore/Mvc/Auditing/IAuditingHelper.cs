using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.AspNetCore.Mvc.Auditing
{
    public interface IAuditingHelper
    {
        public AuditLogInfo CreateAuditLogInfo();
        bool ShouldSaveAudit(MethodInfo methodInfo, bool defaultValue = false, bool ignoreIntegrationServiceAttribute = false);
    }
}
