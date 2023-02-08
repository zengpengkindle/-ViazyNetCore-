using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Modules.Logs;
using Microsoft.AspNetCore.Http;
using ViazyNetCore.Auth.Extensions;

namespace ViazyNetCore.Modules.EventHandler
{
    [InjectionHanlder]
    public class OperationLogEventHandler : IEventHandler<OperationLogEventData>
    {
        private readonly IOperationLogService _logService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthUser? _authUser;

        public OperationLogEventHandler(IOperationLogService logService, IServiceProvider serviceProvider, IHttpContextAccessor httpContextAccessor)
        {
            this._logService = logService;
            this._httpContextAccessor = httpContextAccessor;
            this._authUser = this._httpContextAccessor?.HttpContext?.GetAuthUser();
        }
        public void HandleEvent(OperationLogEventData eventData)
        {
            if(eventData != null && eventData.Data != null)
            {
                eventData.Data.CreateTime = eventData.EventTime;
                if(this._authUser != null)
                {
                    eventData.Data.OperateUserId = this._authUser.UserKey;
                    eventData.Data.Operator = this._authUser.UserName;
                   
                }
                if(this._httpContextAccessor != null)
                {
                    eventData.Data.OperatorIP = this._httpContextAccessor.HttpContext?.GetRequestIP();
                }
                this._logService.AddOperationLog(eventData.Data);
            }
        }
    }
}
