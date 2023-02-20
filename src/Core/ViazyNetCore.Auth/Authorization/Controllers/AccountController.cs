using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ViazyNetCore.Auth;
using ViazyNetCore.Auth.Authorization.Controllers;
using ViazyNetCore.Auth.Authorization.ViewModels;
using ViazyNetCore.Auth.Jwt;
using ViazyNetCore.Authorization.Modules;
using ViazyNetCore.Dtos;
using ViazyNetCore.Modules;

namespace ViazyNetCore.Controllers.Authorization
{
    /// <summary>
    /// 账号管理
    /// </summary>
    public class AccountController : DynamicControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEventBus _eventBus;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly TokenProvider _tokenProvider;

        public AccountController(IUserService userService, IEventBus eventBus, IHttpContextAccessor httpContextAccessor, TokenProvider tokenProvider)
        {
            this._userService = userService;
            this._eventBus = eventBus;
            this._httpContextAccessor = httpContextAccessor;
            this._tokenProvider = tokenProvider;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<UserTokenModel> LoginAsync([Required][FromBody] UserLoginArgs args, [FromServices] IPermissionService permissionService)
        {
            var ip = this._httpContextAccessor.HttpContext!?.GetRequestIP();
            //OperationLog operationLog = new OperationLog
            //{
            //    CreateTime = DateTime.Now,
            //    OperateUserId = args.Username,
            //    OperationType = "登录",
            //    OperatorType = OperatorTypeEnum.Bms,
            //    OperatorIP = ip,
            //};
            if (args.Mark.IsNotNull() && args.Mark != "tools")
            {
                throw new ApiException("无效登录方式!");
            }
            try
            {
                //using (_lockProvider.Lock<UserLoginArgs>(args.Username))
                {
                    var identity = await this._userService.GetUserLoginIdentityAsync(args, ip, false);
                    var permissions = await permissionService.ResolveUserPermission(identity.Id);
                    var token = await this._tokenProvider.IssueToken(identity.Id, identity.Username, permissions.Select(p => p.PermissionItemKey).Distinct().ToArray());
                    //登陆成功，清空缓存
                    _userService.ClearCache(args.Username);

                    //operationLog.OperateUserId = identity.Id;
                    //operationLog.ObjectName = identity.Nickname;
                    //operationLog.ObjectId = identity.Id;
                    //operationLog.Description = $"登录用户：{args.Username},登陆成功";

                    return new UserTokenModel
                    {
                        AccessToken = token.AccessToken,
                        ExpiresIn = token.ExpiresIn,
                        Nickname = identity.Nickname,
                        Permissions = permissions.Select(p => p.PermissionItemKey).Distinct().ToArray()
                    };
                }
            }
            catch (ApiException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                //operationLog.Description = $"登录用户：{args.Username},登陆失败!{ex.Message}";
                throw new ApiException("登录失败");
            }
            finally
            {
                //this._eventBus.Publish(new OperationLogEventData(operationLog));
            }
        }


        [ApiTitle("获取用户标识")]
        [HttpPost]
        public Task<AuthUser> GetIdentityAsync()
        {
            return Task.FromResult(this._httpContextAccessor.HttpContext!.GetAuthUser());
        }

        [AllowAnonymous]
        [HttpPost]
        public Task LogoutAsync()
        {
            this._tokenProvider.RemoveToken(this._httpContextAccessor.HttpContext!.User.GetUserId());
            return Task.CompletedTask;
        }

        [Authorize, ApiTitle("修改密码")]
        [HttpPost]
        public async Task<bool> ModifyPasswordAsync([Required] UserModifyPasswordArgs args)
        {
            var authUser = this._httpContextAccessor.HttpContext!.GetAuthUser();
            //OperationLog operationLog = new OperationLog(this._httpContextAccessor.HttpContext!.GetRequestIP(), authUser!.UserKey, authUser.UserName, OperatorTypeEnum.Bms)
            //{
            //    ObjectName = $"{authUser.UserName}",
            //    ObjectId = authUser.UserKey,
            //    OperationType = $"用户密码修改",
            //    Description = $"用户名：{authUser.UserName}",
            //    LogLevel = LogRecordLevel.Warning
            //};

            try
            {
                var res = await this._userService.ModifyPasswordAsync(authUser.UserKey, args);
                //if (res)
                //{
                //    this._eventBus.Publish(new OperationLogEventData()
                //    {
                //        Data = operationLog,
                //        EventTime = DateTime.Now
                //    });
                //}
                return res;
            }
            finally
            {
                this._tokenProvider.RemoveToken(this._httpContextAccessor.HttpContext!.User.GetUserId());
            }
        }
    }
}
