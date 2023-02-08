using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules
{
    [Injection]
    public interface IPageGroupService
    {
        /// <summary>
        /// 添加或修改模型。
        /// </summary>
        /// <param name="context">数据库上下文。</param>
        /// <param name="model">模型。</param>
        /// <returns>模型的编号。</returns>
        Task<long> ManageAsync(PageGroupModel model);

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
        /// <param name="status">状态，为空表示查询所有。</param>
        /// <returns>模型的集合。</returns>
        Task<List<PageGroupModel>> FindAllAsync(ComStatus? status);
    }
}
