using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.Auth;
using ViazyNetCore.Domain;
using ViazyNetCore.Filter.Descriptor;
using ViazyNetCore.Manage.WebApi.ViewModel;
using ViazyNetCore.Model;
using ViazyNetCore.Modules;

namespace ViazyNetCore.Manage.WebApi.Controllers.Authorization
{
    /// <summary>
    /// 角色管理
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController:ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IPageGroupService _pageGroupService;
        private readonly IPageService _pageService;
        private readonly IEventBus _eventBus;
        private readonly IApiManager _apiManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RoleController(IRoleService roleService, IPageGroupService pageGroupService, IPageService pageService
            , IEventBus eventBus
            , IApiManager apiManager, IHttpContextAccessor httpContextAccessor)
        {
            this._roleService = roleService;
            this._pageGroupService = pageGroupService;
            this._pageService = pageService;
            this._eventBus = eventBus;
            this._apiManager = apiManager;
            this._httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 所有查询
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [ApiTitle("所有查询")]
        [Route("findAll"), HttpPost]
        public Task<PageData<RoleFindAllModel>> FindAllAsync([Required] RoleFindAllArgs args)
        {
            return this._roleService.FindAllAsync(args);
        }

        /// <summary>
        /// 单个查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ApiTitle("单个查询")]
        [Route("find"), HttpPost]
        public Task<RoleModel> FindAsync([Required] long id)
        {
            return this._roleService.FindAsync(id);
        }

        /// <summary>
        /// 管理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ApiTitle("管理")]
        [Route("manage"), HttpPost]
        public Task<long> ManageAsync([Required] RoleModel model)
        {
            var describe = model.Id != 0 ? "修改" : "添加";
            var result = this._roleService.ManageAsync(model);
            var authUser = this.HttpContext.GetAuthUser();
            OperationLog operationLog = new OperationLog(this.HttpContext.GetRequestIP(), authUser.UserKey, authUser.UserName, OperatorTypeEnum.Bms)
            {
                ObjectName = $"角色{describe}",
                ObjectId = model.Id.ToString(),
                OperationType = $"{describe}角色",
                Description = $"角色名：{model.Name}",
                LogLevel = LogRecordLevel.Warning
            };
            this._eventBus.Publish(new OperationLogEventData()
            {
                Data = operationLog,
                EventTime = DateTime.Now
            });
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ApiTitle("删除")]
        [Route("remove"), HttpPost]
        public Task RemoveAsync([Required] long id)
        {
            var authUser = this.HttpContext.GetAuthUser();
            OperationLog operationLog = new OperationLog(this.HttpContext.GetRequestIP(), authUser.UserKey, authUser.UserName, OperatorTypeEnum.Bms)
            {
                ObjectName = "角色删除",
                ObjectId = id.ToString(),
                OperationType = "删除角色",
                Description = $"角色编号:{id}",
                LogLevel = LogRecordLevel.Warning
            };
            this._eventBus.Publish(new OperationLogEventData()
            {
                Data = operationLog,
                EventTime = DateTime.Now
            });
            return this._roleService.RemoveAsync(id);
        }

        /// <summary>
        /// 所有查询(启用状态)
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [ApiTitle("所有查询(启用状态)")]
        [Route("findAllEnabed"), HttpPost]
        public Task<PageData<RoleSimpleModel>> FindAllEnabledAsync([Required] RoleFindAllEnabledArgs args)
        {
            return this._roleService.FindAllEnabledAsync(args);
        }

        /// <summary>
        /// 设置角色页面
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="pageIds"></param>
        /// <returns></returns>
        [ApiTitle("设置角色页面")]
        [Route("setRolePageIds"), HttpPost]
        public async Task SetRolePageIdsAsync([Required] long roleId, [Required] long[] pageIds)
        {
            await this._roleService.SetRolePageIdsAsync(roleId, pageIds);
            var authUser = this.HttpContext.GetAuthUser();
            OperationLog operationLog = new OperationLog(this.HttpContext.GetRequestIP(), authUser.UserKey, authUser.UserName, OperatorTypeEnum.Bms)
            {
                ObjectName = "角色页面",
                ObjectId = roleId.ToString(),
                OperationType = "修改页面权限",
                Description = $"角色编号：{roleId}",
                LogLevel = LogRecordLevel.Warning
            };
            this._eventBus.Publish(new OperationLogEventData()
            {
                Data = operationLog,
                EventTime = DateTime.Now
            });
        }

        /// <summary>
        /// 查询角色页面
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [ApiTitle("查询角色页面")]
        [Route("getRolePages"), HttpPost]
        public async Task<RolePagesModel> GetRolePagesAsync([Required] long roleId)
        {
            var groups = await this._pageGroupService.FindAllAsync(ComStatus.Enabled);
            var pages = await this._pageService.GetRolePagesAsync(roleId);
            var checkedKeys = await this._roleService.GetRolePagesIdsAsync(roleId);
            return new RolePagesModel
            {
                Groups = groups,
                Pages = pages,
                CheckedKeys = checkedKeys
            };
        }

        /// <summary>
        /// 设置角色接口
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        [ApiTitle("设置角色接口")]
        [Route("setRoleApiIds"), HttpPost]
        public async Task SetRoleApiIdsAsync([Required] long roleId, [Required] string[] itemIds)
        {
            await this._roleService.SetRoleApiIdsAsync(roleId, itemIds);
            var authUser = this.HttpContext.GetAuthUser();
            OperationLog operationLog = new OperationLog(this.HttpContext.GetRequestIP(), authUser.UserKey, authUser.UserName, OperatorTypeEnum.Bms)
            {
                ObjectName = "角色接口",
                ObjectId = roleId.ToString(),
                OperationType = "修改接口权限",
                Description = $"角色编号：{roleId}",
                LogLevel = LogRecordLevel.Warning
            };
            this._eventBus.Publish(new OperationLogEventData()
            {
                Data = operationLog,
                EventTime = DateTime.Now
            });
        }

        /// <summary>
        /// 查询角色接口
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [ApiTitle("查询角色接口")]
        [Route("getRoleApis"), HttpPost]
        public async Task<RoleApisModel> GetRoleApisAsync([Required] long roleId)
        {
            return new RoleApisModel
            {
                Apis = this._apiManager.GetApiDescriptors(),
                CheckedKeys = await this._roleService.GetRoleApiIdsAsync(roleId),
            };
        }

        /// <summary>
        /// 角色权限点列表
        /// </summary>
        /// <returns></returns>
        [ApiTitle("角色权限点列表")]
        [Route("getPermissionCodeList"), HttpPost]
        public List<KeyValuePair<BMSPermissionCode, string>> GetPermissionCodeListAsync()
        {
            List<KeyValuePair<BMSPermissionCode, string>> res = new List<KeyValuePair<BMSPermissionCode, string>>();

            var items = Enum.GetValues(typeof(BMSPermissionCode)).Cast<BMSPermissionCode>();
            foreach (int item in items)
            {
                var it = (BMSPermissionCode)item;
                res.Add(new KeyValuePair<BMSPermissionCode, string>(it, it.ToDescriptionOrString()));
            }
            return res;
        }
    }
}
