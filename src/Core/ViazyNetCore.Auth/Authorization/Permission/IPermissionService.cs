namespace ViazyNetCore.Authorization.Modules
{
    [Injection]
    public interface IPermissionService
    {
        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <returns></returns>
        Task<List<PermissionKey>> GetAll();

        /// <summary>
        /// 获取用户角色对应的权限设置
        /// </summary>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="ownerType">拥有者所属类别</param>
        /// <returns>返回roleName对应的权限设置</returns>
        Task<IEnumerable<BmsOwnerPermission>> GetPermissionsInUserRole(string ownerId, OwnerType ownerType);

        /// <summary>
        /// 更新权限规则
        /// </summary>
        /// <param name="permissionItemKeys">待更新的权限项目规则集合</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="ownerType">拥有者所属类别</param>
        Task UpdatePermissionsInUserRole(IEnumerable<string> permissionItemKeys, string ownerId, OwnerType ownerType);

        /// <summary>
        /// 移出权限
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        Task RemoveMenu(string menuId);

        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <returns></returns>
        Task<List<BmsMenus>> GetAllMenu();

        /// <summary>
        /// 获取所有有效菜单
        /// </summary>
        /// <returns></returns>
        Task<List<BmsMenus>> GetAllEnableMenu();

        Task<BmsMenus> GetMenu(string id);

        Task<string> UpdateMenus(BmsMenus menu);

        Task AddPermission(string name, string key);

        Task<BmsPermission> GetPermissionByPermissionKey(string key);

        Task<bool> ExistsPermissionByPermissionKey(string key);

        Task UpdateMenusInPermission(string permissionItemKey, string[] menuIds);

        Task<IEnumerable<BmsMenus>> GetMenusInPermissionKey(string permissionItemKey);

        Task<IEnumerable<BmsMenus>> GetMenusInPermissionKeys(string[] permissionItemKeys);

        /// <summary>
        /// 解析用户的所有权限
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        Task<IEnumerable<BmsOwnerPermission>> ResolveUserPermission(string userId);

        /// <summary>
        /// 检查用户是否有权限进行某项操作
        /// </summary>
        /// <param name="currentUser">当前用户</param>
        /// <param name="permissionItemKey">权限项目标识</param>
        /// <returns>有权限操作返回true，否则返回false</returns>
        Task<bool> Check(IUser<string> currentUser, string permissionItemKey);
        /// <summary>
        /// 检查用户是否有权限进行某项操作
        /// </summary>
        /// <param name="currentUser">当前用户</param>
        /// <param name="permissionItemKey">权限项目标识</param>
        /// <returns>有权限操作返回true，否则返回false</returns>
        Task<bool> Check(IUser<string> currentUser, string[] permissionItemKeys);

    }
}
