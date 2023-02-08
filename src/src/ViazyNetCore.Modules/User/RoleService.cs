using System.Transactions;

namespace ViazyNetCore.Modules
{
    /// <summary>
    /// 表示一个角色的服务。
    /// </summary>
    [Injection]
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRolePageRepository _rolePageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IPageRepository _pageRepository;
        public RoleService(IRoleRepository roleRepository, IRolePageRepository rolePageRepository, IUserRepository userRepository, IRolePermissionRepository rolePermissionRepository, IPageRepository pageRepository)
        {
            this._rolePageRepository = rolePageRepository;
            this._roleRepository = roleRepository;
            this._rolePermissionRepository = rolePermissionRepository;
            this._userRepository = userRepository;
            this._pageRepository = pageRepository;
        }

        /// <summary>
        /// 添加或修改模型。
        /// </summary>
        /// <param name="model">模型。</param>
        /// <returns>模型的编号。</returns>
        public async Task<long> ManageAsync(RoleModel model)
        {
            if(model.Id == 0)
            {
                var role = new BmsRole
                {
                    Id = model.Id,
                    Name = model.Name,
                    Status = model.Status,
                    CreateTime = DateTime.Now,
                    ModifyTime = DateTime.Now,
                    ExtraData = model.ExtraData,
                };
                await _roleRepository.AddByBmsRoleAsync(role);
                model.Id = role.Id;
            }
            else
            {
                await _roleRepository.ModifyByRoleModelAsync(model);
            }
            return model.Id;
        }

        /// <summary>
        /// 彻底删除模型。
        /// </summary>
        /// <param name="id">模型的编号。</param>
        /// <returns>异步操作。</returns>
        public async Task RemoveAsync(long id)
        {
            if(await _userRepository.ExistByIdAsync(id)) throw new ApiException("此角色绑定了用户，请先移除绑定关系。");
            await _roleRepository.RemoveByIdAsync(id);
        }

        /// <summary>
        /// 判定指定的模型编号是否存在。
        /// </summary>
        /// <param name="id">模型编号。</param>
        /// <returns>存在返回 true 值，否则返回 false 值。</returns>
        public Task<bool> ExistsAsync(long id)
        {
            return _roleRepository.ExistsAsync(id);
        }

        /// <summary>
        /// 查找指定编号的模型。
        /// </summary>
        /// <param name="id">模型编号。</param>
        /// <returns>模型。</returns>
        public Task<RoleModel> FindAsync(long id)
        {
            return _roleRepository.FindByIdAsync(id);
        }

        /// <summary>
        /// 查询所有模型。
        /// </summary>
        /// <param name="args">查询参数。</param>
        /// <returns>模型的集合。</returns>
        public Task<PageData<RoleFindAllModel>> FindAllAsync(RoleFindAllArgs args)
        {
            return _roleRepository.FindAllAsync(args.NameLike, args.Status, args);
        }

        /// <summary>
        /// 查询所有启用的模型。
        /// </summary>
        /// <param name="args">查询参数。</param>
        /// <returns>模型的集合。</returns>
        public async Task<PageData<RoleSimpleModel>> FindAllEnabledAsync(RoleFindAllEnabledArgs args)
        {
            var data =await this._roleRepository.FindAllAsync(args.NameLike, ComStatus.Enabled, args);
            List<RoleSimpleModel> list = new List<RoleSimpleModel>();
            foreach(var item in data.Rows)
            {
                list.Add(item.CopyTo<RoleSimpleModel>());
            }
            return PageData.Create(list.ToArray(), data.Total);
        }

        /// <summary>
        /// 获取角色的所有页面权限。
        /// </summary>
        /// <param name="roleId">角色编号。</param>
        /// <returns>页面的编号集合。</returns>
        public Task<List<long>> GetRolePagesIdsAsync(long roleId)
        {
            return _rolePageRepository.GetRolePagesIdsAsync(roleId);
        }

        /// <summary>
        /// 设置角色的所有权限页面。
        /// </summary>
        /// <param name="roleId">角色编号。</param>
        /// <param name="pageIds">页面的编号集合。</param>
        /// <returns>异步操作。</returns>
        public async Task SetRolePageIdsAsync(long roleId, long[] pageIds)
        {
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            await _rolePageRepository.RemoveRolePageByIdAsync(roleId);
            var createTime = DateTime.Now;
            foreach(var pageId in pageIds)
            {
                if(pageId != 0)
                    await _rolePageRepository.AddRolePageAsync(roleId, pageId, createTime);
            }
            transaction.Complete();
            _pageRepository.RemovePageCacheByRoleId(roleId);
        }

        /// <summary>
        /// 获取角色的所有接口权限。
        /// </summary>
        /// <param name="roleId">角色编号。</param>
        /// <returns>接口的编号集合。</returns>
        public Task<List<string>> GetRoleApiIdsAsync(long roleId)
        {
            return _roleRepository.GetPermissionIdsByRoleIdCacheAsync(roleId);
        }

        /// <summary>
        /// 设置角色的所有接口权限。
        /// </summary>
        /// <param name="roleId">角色编号。</param>
        /// <param name="itemIds">接口的编号集合。</param>
        /// <returns>异步操作。</returns>
        public async Task SetRoleApiIdsAsync(long roleId, string[] itemIds)
        {
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            await this._rolePermissionRepository.RemoveRolePermissionByIdAsync(roleId);
            this._roleRepository.ClearPermissionIdsByRoleIdCache(roleId);
            var createTime = DateTime.Now;
            foreach(var itemId in itemIds)
            {
                if(itemId.IsNotNull())
                    await _roleRepository.AddRolePermissionByIdAsync(roleId, itemId, createTime);
            }
            transaction.Complete();
        }

        /// <summary>
        /// 检查角色权限点
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <param name="value">枚举，权限点</param>
        /// <returns></returns>
        public async Task<bool> CheckPermission(long roleId, object value)
        {
            if(roleId == Globals.ADMIN_ROLE_ID) return true;
            if(value == null) return false;
            if(!value.GetType().IsEnum) throw new ArgumentException("An Enumeration type is required.", "value");

            Type type = value.GetType();

            if(type != typeof(BMSPermissionCode) && type != typeof(MerchantPermissionCode) && type != typeof(AgentPermissionCode))
                throw new ArgumentException("An Enumeration type is required.", "value");

            RoleModel role = await this.FindAsync(roleId);

            if(role.ExtraData.IsNotNull())
            {
                if(role.ExtraData!.Contains(((int)BMSPermissionCode.All).ToString())
                    || role.ExtraData.Contains(((int)MerchantPermissionCode.All).ToString())
                    || role.ExtraData.Contains(((int)AgentPermissionCode.All).ToString()))
                    return true;

                if(role.ExtraData.Contains(((int)value).ToString()))
                    return true;
            }

            return false;
        }
    }
}
