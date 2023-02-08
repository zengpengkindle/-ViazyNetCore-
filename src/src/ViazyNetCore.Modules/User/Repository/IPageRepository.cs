using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ViazyNetCore.Modules.Repository
{
    [Injection]
    /// <summary>
    /// 表示一个页面服务仓储接口。
    /// </summary>
    public interface IPageRepository : IBaseRepository<BmsPage, long>
    {
        /// <summary>
        /// 根据PageModel添加模型。
        /// </summary>
        /// <param name="model">PageModel模型。</param>
        /// <returns>模型的编号。</returns>
        Task AddPageGroupModelAsync(BmsPage group, long roleId);

        /// <summary>
        /// 修改模型。
        /// </summary>
        /// <param name="model">PageModel模型。</param>
        /// <returns></returns>
        Task ModifyPageGroupModelAsync(PageModel model, long roleId);

        /// <summary>
        /// 彻底删除模型。
        /// </summary>
        /// <param name="id">模型的编号。</param>
        /// <returns>异步操作。</returns>
        Task RemoveByIdAsync(long id);

        /// <summary>
        /// 根据PageFindAllArgs查询所有模型。
        /// </summary>
        /// <param name="args">查询参数。</param>
        /// <returns>模型的集合。</returns>
        Task<PageData<PageFindAllModel>> FindAllByPageFindAllArgsAsync(PageFindAllArgs args);

        /// <summary>
        /// 获取基于角色的页面。
        /// </summary>
        /// <param name="roleId">角色编号，空值表示获取所有。</param>
        /// <returns>模型的集合。</returns>
        Task<List<PageSimpleModel>> GetRolePagesAsync(long roleId);

        /// <summary>
        /// 根据id获取是否存在页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> FindPageAsync(long id);
        /// <summary>
        /// 根据roleId移除缓存
        /// </summary>
        /// <param name="roleId"></param>
        void RemovePageCacheByRoleId(long roleId);
    }
}
