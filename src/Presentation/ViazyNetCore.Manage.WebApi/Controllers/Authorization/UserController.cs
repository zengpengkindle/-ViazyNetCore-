using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ViazyNetCore.Domain;
using ViazyNetCore.Manage.WebApi.ViewModel;
using ViazyNetCore.Model;
using ViazyNetCore.Modules;

namespace ViazyNetCore.Manage.WebApi.Controllers.Authorization
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEventBus _eventBus;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRoleService _roleService;
        private readonly IPageGroupService _pageGroupService;
        private readonly IPageService _pageService;

        public UserController(IUserService userService, IRoleService roleService, IPageGroupService pageGroupService, IPageService pageService, IEventBus eventBus, IHttpContextAccessor httpContextAccessor)
        {
            this._userService = userService;
            this._roleService = roleService;
            this._pageGroupService = pageGroupService;
            this._pageService = pageService;
            this._eventBus = eventBus;
            this._httpContextAccessor = httpContextAccessor;
        }

        [ApiTitle("所有查询")]
        [Route("findAll"), HttpPost]
        public Task<PageData<UserFindAllModel>> FindAllAsync([Required] UserFindAllArgs args)
        {
            return this._userService.FindAllAsync(args);
        }

        [ApiTitle("单个查询")]
        [Route("find"), HttpPost]
        public Task<UserFindModel> FindAsync([Required] long id)
        {
            return this._userService.FindAsync(id);
        }

        [ApiTitle("管理")]
        [Route("manage"), HttpPost]
        public async Task<UserManageDto> ManageAsync([Required] UserModel model)
        {
            if (!await this._roleService.ExistsAsync(model.RoleId)) throw new ApiException("角色不存在。");
            var describe = model.Id != 0 ? "修改" : "添加";

            string randPwd = model.Id != 0 ? null : Globals.GetRandomPassword();
            var userId = await this._userService.ManageAsync(model, randPwd);

            var authUser = this.HttpContext.GetAuthUser();
            OperationLog operationLog = new OperationLog(this.HttpContext.GetRequestIP(), authUser.UserKey, authUser.UserName, OperatorTypeEnum.Bms)
            {
                ObjectName = $"用户{describe}",
                ObjectId = model.Id.ToString(),
                OperationType = $"{describe}用户",
                Description = $"用户名：{model.Username}",
                LogLevel = LogRecordLevel.Warning
            };
            this._eventBus.Publish(new OperationLogEventData()
            {
                Data = operationLog,
                EventTime = DateTime.Now
            });

            return new UserManageDto() { UserId = userId, UserName = model.Username, Password = randPwd };
        }

        [ApiTitle("删除")]
        [Route("remove"), HttpPost]
        public async Task RemoveAsync([Required] long id)
        {
            await this._userService.RemoveAsync(id);

            var authUser = this.HttpContext.GetAuthUser();
            OperationLog operationLog = new OperationLog(this.HttpContext.GetRequestIP(), authUser.UserKey, authUser.UserName, OperatorTypeEnum.Bms)
            {
                ObjectName = "用户删除",
                ObjectId = id.ToString(),
                OperationType = "删除用户",
                Description = $"用户编码:{id}",
                LogLevel = LogRecordLevel.Warning
            };
            this._eventBus.Publish(new OperationLogEventData()
            {
                Data = operationLog,
                EventTime = DateTime.Now
            });
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ApiTitle("重置密码")]
        [Route("restPassword"), HttpPost]
        public async Task<string> ResetPasswordAsync(long id)
        {
            var authUser = this.HttpContext.GetAuthUser();
            var res = await this._userService.ResetPasswordAsync(id);
            OperationLog operationLog = new OperationLog(this.HttpContext.GetRequestIP(), authUser.UserKey, authUser.UserName, OperatorTypeEnum.Bms)
            {
                ObjectName = "重置密码",
                ObjectId = id.ToString(),
                OperationType = "重置密码",
                Description = $"编码:{id}"
            };
            this._eventBus.Publish(new OperationLogEventData()
            {
                Data = operationLog,
                EventTime = DateTime.Now
            });


            return res;
        }

        [ApiTitle("重置登录限制")]
        [Route("resetTime"), HttpPost]
        public void ResetTime(string username)
        {
            this._userService.ClearCache(username);
        }

        /// <summary>
        /// 获取用户路由
        /// </summary>
        /// <returns></returns>
        [Route("getRouter"), HttpPost]
        public async Task<List<MenuPageItem>> GetRouterAsync()
        {
            var userRoles = this.HttpContext.GetAuthUserRoleIds();

            var groups = await this._pageGroupService.FindAllAsync(ComStatus.Enabled);
            var pages = await this._pageService.GetRolePagesAsync(userRoles.First());

            var result = groups.Select(g => new MenuPageItem
            {
                IsGroup = true,
                Icon = g.Icon,
                Status = g.Status,
                Title = g.Title,
                Children = pages.Where(p => p.GroupId == g.Id).Select(p => new MenuPageItem
                {
                    Icon = p.Icon,
                    IsGroup = false,
                    Status = ComStatus.Enabled,
                    Title = p.Title,
                    Url = p.Url
                }).ToList()
            }).ToList();
            return result;
        }

    }
}
