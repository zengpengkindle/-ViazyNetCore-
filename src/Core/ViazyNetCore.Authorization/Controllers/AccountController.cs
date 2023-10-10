using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.Auth;
using ViazyNetCore.Auth.Authorization.ViewModels;
using ViazyNetCore.Auth.Jwt;
using ViazyNetCore.Authorization.Modules;
using ViazyNetCore.Identity.Domain;
using ViazyNetCore.Modules;

namespace ViazyNetCore.Authorization
{
    /// <summary>
    /// 账号管理
    /// </summary>
    public class AccountController : DynamicControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEventBus _eventBus;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IdentityUserManager _userManager;
        private readonly SignInManager _signInManager;
        private readonly TokenProvider _tokenProvider;

        public AccountController(IUserService userService
            , IEventBus eventBus
            , IHttpContextAccessor httpContextAccessor
            , IdentityUserManager identityUserManager
            , SignInManager signInManager
            , TokenProvider tokenProvider)
        {
            this._userService = userService;
            this._eventBus = eventBus;
            this._httpContextAccessor = httpContextAccessor;
            this._userManager = identityUserManager;
            this._signInManager = signInManager;
            this._tokenProvider = tokenProvider;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<UserTokenDto> LoginAsync([Required][FromBody] UserLoginInputDto args, [FromServices] IPermissionService permissionService)
        {
            var ip = this._httpContextAccessor.HttpContext!?.GetRequestIP();
            OperationLog operationLog = new OperationLog
            {
                CreateTime = DateTime.Now,
                OperateUserId = args.Username,
                OperationType = "登录",
                OperatorType = OperatorType.Bms,
                OperatorIP = ip,
            };
            try
            {
                using (GA.Lock("UserLoginArgs" + args.Username))
                {
                    var user = await this._userManager.FindByNameAsync(args.Username);
                    if (user == null)
                    {
                        throw new ApiException("用户不存在");
                    }
                    //var identity = await this._userService.GetUserLoginIdentityAsync(args, ip, false);
                    var signInResult = await this._signInManager.CheckPasswordSignInAsync(user, args.Password, true);
                    if (!signInResult.Succeeded)
                    {
                        if (signInResult.IsLockedOut)
                        {
                            throw new ApiException($"用户因密码错误次数过多而被锁定 {_userManager.Options.Lockout.DefaultLockoutTimeSpan.TotalMinutes} 分钟，请稍后重试");
                        }
                        if (signInResult.IsNotAllowed)
                        {
                            throw new ApiException("不允许登录。");
                        }
                        throw new ApiException("登录失败，用户名或账号无效。");
                    }

                    var identity = await this._userManager.FindByNameAsync(args.Username);

                    var permissions = await permissionService.ResolveUserPermission(identity.Id);
                    var token = await this._tokenProvider.IssueToken(identity, AuthUserType.Normal, permissions.Select(p => p.PermissionItemKey).Distinct().ToArray());
                    //登陆成功，清空缓存
                    _userService.ClearCache(args.Username);

                    operationLog.OperateUserId = identity.Id.ToString();
                    operationLog.ObjectName = identity.Nickname;
                    operationLog.ObjectId = identity.Id.ToString();
                    operationLog.Description = $"登录用户：{args.Username},登陆成功";

                    return new UserTokenDto
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
                operationLog.Description = $"登录用户：{args.Username},登陆失败!{ex.Message}";
                throw new ApiException(ex);
            }
            finally
            {
                this._eventBus.Publish(new OperationLogEventData(operationLog));
            }
        }


        [ApiTitle("获取用户标识")]
        [HttpPost]
        public Task<AuthUser> GetIdentityAsync()
        {
            return Task.FromResult(this._httpContextAccessor.GetAuthUser()!);
        }

        [ApiTitle("获取用户个人信息")]
        [HttpPost]
        public async Task<UserBaseDto> GetUserInfoAsync()
        {
            return await this._userService.FindAsync(this._httpContextAccessor.GetAuthUser()!.Id);
        }

        [AllowAnonymous]
        [HttpPost]
        public Task LogoutAsync()
        {
            this._tokenProvider.RemoveToken(this._httpContextAccessor.GetAuthUser()!.Id);
            return Task.CompletedTask;
        }

        [Authorize, ApiTitle("修改密码")]
        [HttpPost]
        public async Task<bool> ModifyPasswordAsync([Required] UserModifyPasswordEditDto args)
        {
            var authUser = this._httpContextAccessor.HttpContext!.GetAuthUser();
            OperationLog operationLog = new OperationLog(this._httpContextAccessor.HttpContext!.GetRequestIP(), authUser!.Id.ToString(), authUser.Username, OperatorType.Bms)
            {
                ObjectName = $"{authUser.Username}",
                ObjectId = authUser.Id.ToString(),
                OperationType = $"用户密码修改",
                Description = $"用户名：{authUser.Username}",
                LogLevel = LogRecordLevel.Warning
            };

            try
            {
                var user = await this._userManager.FindByIdAsync(authUser!.Id.ToString());
                var res = await this._userManager.ChangePasswordAsync(user, args.OldPassword, args.NewPassword);
                if (res.Succeeded)
                {
                    this._eventBus.Publish(new OperationLogEventData(operationLog));
                    return true;
                }
                else
                {
                    throw new ApiException(res.Errors?.First().Description ?? "修改失败");
                }
            }
            finally
            {
                this._tokenProvider.RemoveToken(this._httpContextAccessor.GetAuthUser()!.Id);
            }
        }

        [Authorize, ApiTitle("修改个人信息")]
        [HttpPost]
        public async Task<bool> ModifyAvatarAsync([Required] UserUpdateDto args)
        {
            var authUser = this._httpContextAccessor.HttpContext!.GetAuthUser();
            OperationLog operationLog = new OperationLog(this._httpContextAccessor.HttpContext!.GetRequestIP(), authUser!.Id.ToString(), authUser.Username, OperatorType.Bms)
            {
                ObjectName = $"{authUser.Username}",
                ObjectId = authUser.Id.ToString(),
                OperationType = $"用户修改个人信息",
                Description = $"用户名：{authUser.Username}",
                LogLevel = LogRecordLevel.Warning
            };

            try
            {
                var res = await this._userService.UpdateUserInfoAsync(authUser.Id, args);
                if (res)
                {
                    this._eventBus.Publish(new OperationLogEventData(operationLog));
                }
                return res;
            }
            finally
            {
                //this._tokenProvider.RemoveToken(this._httpContextAccessor.GetAuthUser()!.Id);
            }
        }
    }
}
