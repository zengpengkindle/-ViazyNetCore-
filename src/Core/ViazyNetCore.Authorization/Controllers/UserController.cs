﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ViazyNetCore.Auth;
using ViazyNetCore.Identity;
using ViazyNetCore.Modules;

namespace ViazyNetCore.Manage.WebApi.Controllers.Authorization
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [ApiPermission("用户管理")]
    public class UserController : DynamicControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEventBus _eventBus;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptions<UserOption> _options;

        public UserController(IUserService userService, IEventBus eventBus, IHttpContextAccessor httpContextAccessor, IOptions<UserOption> options)
        {
            this._userService = userService;
            this._eventBus = eventBus;
            this._httpContextAccessor = httpContextAccessor;
            this._options = options;
        }

        [HttpPost]
        [Permission(PermissionIds.User)]
        public Task<PageData<Dtos.UserDto>> FindAllAsync([Required] UserFindQueryDto args)
        {
            return this._userService.FindAllAsync(args);
        }

        [ApiPermission("单个查询")]
        [HttpPost]
        [Permission(PermissionIds.User)]
        public Task<UserWithGoogleKeyDto> FindAsync([Required] long id)
        {
            return this._userService.FindAsync(id);
        }

        [HttpPost]
        [Permission(PermissionIds.User)]
        public async Task<UserManageDto> ManageAsync([Required] Dtos.UserBaseDto model)
        {
            //if (!await this._roleService.ExistsAsync(model.RoleId)) throw new ApiException("角色不存在。");
            var describe = model.Id == 0 ? "添加" : "修改";

            var randPwd = model.Id == 0 ? "A121212" : null;// Globals.GetRandomPassword();
            var userId = await this._userService.ManageAsync(model, randPwd);

            var authUser = this._httpContextAccessor.HttpContext!.GetAuthUser();
            OperationLog operationLog = new OperationLog(this._httpContextAccessor.HttpContext!.GetRequestIP(), authUser.Id.ToString(), authUser.Username, OperatorType.Bms)
            {
                ObjectName = $"用户{describe}",
                ObjectId = model.Id.ToString(),
                OperationType = $"{describe}用户",
                Description = $"用户名：{model.Username}",
                LogLevel = LogRecordLevel.Warning
            };
            this._eventBus.Publish(new OperationLogEventData(operationLog));

            return new UserManageDto() { UserId = userId, UserName = model.Username, Password = randPwd };
        }

        [HttpPost]
        [Permission(PermissionIds.User)]
        public async Task<bool> RemoveAsync([Required] long id)
        {
            await this._userService.RemoveAsync(id);

            var authUser = this._httpContextAccessor.HttpContext!.GetAuthUser();
            OperationLog operationLog = new OperationLog(this._httpContextAccessor.HttpContext!.GetRequestIP(), authUser.Id.ToString(), authUser.Username, OperatorType.Bms)
            {
                ObjectName = "用户删除",
                ObjectId = id.ToString(),
                OperationType = "删除用户",
                Description = $"用户编码:{id}",
                LogLevel = LogRecordLevel.Warning
            };
            this._eventBus.Publish(new OperationLogEventData(operationLog));
            return true;
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Permission(PermissionIds.User)]
        public async Task<string> ResetPasswordAsync(long id)
        {
            var authUser = this._httpContextAccessor.HttpContext!.GetAuthUser();
            var res = await this._userService.ResetPasswordAsync(id, _options.Value.GetRandomPassword);
            OperationLog operationLog = new OperationLog(this._httpContextAccessor.HttpContext!.GetRequestIP(), authUser.Id.ToString(), authUser.Username, OperatorType.Bms)
            {
                ObjectName = "重置密码",
                ObjectId = id.ToString(),
                OperationType = "重置密码",
                Description = $"编码:{id}"
            };
            this._eventBus.Publish(new OperationLogEventData(operationLog));
            return res;
        }

        [HttpPost]
        [Permission(PermissionIds.User)]
        public bool ResetTime(string username)
        {
            this._userService.ClearCache(username);
            return true;
        }
    }
}
