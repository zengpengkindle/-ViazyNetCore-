using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules
{
    [Injection]
    public interface IPageService
    {
        /// <summary>
        /// 添加或修改模型。
        /// </summary>
        /// <param name="context">数据库上下文。</param>
        /// <param name="model">模型。</param>
        /// <returns>模型的编号。</returns>
        Task<long> ManageAsync(PageModel model, long roleId);

        /// <summary>
        /// 彻底删除模型。
        /// </summary>
        /// <param name="context">数据库上下文。</param>
        /// <param name="id">模型的编号。</param>
        /// <returns>异步操作。</returns>
        Task RemoveAsync(long id);

        /// <summary>
        /// 查询所有模型。
        /// </summary>
        /// <param name="context">数据库上下文。</param>
        /// <param name="args">查询参数。</param>
        /// <returns>模型的集合。</returns>
        Task<PageData<PageFindAllModel>> FindAllAsync(PageFindAllArgs args);

        /// <summary>
        /// 获取基于角色的页面。
        /// </summary>
        /// <param name="context">数据库上下文。</param>
        /// <param name="roleId">角色编号，空值表示获取所有。</param>
        /// <returns>模型的集合。</returns>
        Task<List<PageSimpleModel>> GetRolePagesAsync(long roleId);
    }
}
