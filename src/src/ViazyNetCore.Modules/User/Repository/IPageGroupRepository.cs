using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.Repository
{
    [Injection]
    /// <summary>
    ///表示页面分组服务仓储接口。
    /// </summary>
    public interface IPageGroupRepository : IBaseRepository<BmsPageGroup,long>
    {
        /// <summary>
        /// 根据PageGroupModel添加模型。
        /// </summary>
        /// <param name="model">PageGroupModel模型。</param>
        /// <returns>模型的编号。</returns>
        Task AddPageGroupModelAsync(BmsPageGroup group);

        /// <summary>
        /// 修改模型。
        /// </summary>
        /// <param name="model">PageGroupModel模型。</param>
        /// <returns></returns>
        Task ModifyPageGroupModelAsync(PageGroupModel model);

        /// <summary>
        /// 根据id查询是否存在分组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> FindPageAsync(long id);

        /// <summary>
        /// 彻底删除模型。
        /// </summary>
        /// <param name="id">模型的编号。</param>
        /// <returns>异步操作。</returns>
        Task RemoveByIdAsync(long id);

        /// <summary>
        /// 查询所有模型。
        /// </summary>
        /// <param name="status">状态，为空表示查询所有。</param>
        /// <returns>模型的集合。</returns>
        Task<List<PageGroupModel>> FindAllAsync(ComStatus? status);
    }
}