﻿using Microsoft.AspNetCore.Http;
using ViazyNetCore.Auth;
using ViazyNetCore.Authorization.Modules;

namespace ViazyNetCore.Modules.EventHandler
{
    [InjectionHanlder]
    public class OperationLogEventHandler : ILocalEventHandler<OperationLogEventData>
    {
        private readonly IOperationLogService _logService;
        private readonly IHttpContextAccessor? _httpContextAccessor;
        private readonly AuthUser? _authUser;

        public OperationLogEventHandler(IOperationLogService logService, IHttpContextAccessor httpContextAccessor)
        {
            this._logService = logService;
            this._httpContextAccessor = httpContextAccessor;
            this._authUser = this._httpContextAccessor?.HttpContext?.GetAuthUser();
        }
        public async Task HandleEventAsync(OperationLogEventData eventData)
        {
            if (eventData != null && eventData.Data != null)
            {
                eventData.Data.CreateTime = eventData.EventTime;
                if (this._authUser != null)
                {
                    eventData.Data.OperateUserId = this._authUser.Id.ToString();
                    eventData.Data.Operator = this._authUser.Username;

                }
                if (this._httpContextAccessor != null)
                {
                    eventData.Data.OperatorIP = this._httpContextAccessor.HttpContext?.GetRequestIP();
                }
                await this._logService.AddOperationLog(eventData.Data);
            }
        }
    }
}
