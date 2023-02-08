using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.Repository
{
    /// <summary>
    /// 表示页面分组服务仓储。
    /// </summary>
    [Injection]
    public class PageGroupRepository : DefaultRepository<BmsPageGroup,long>, IPageGroupRepository
    {
        public PageGroupRepository(IFreeSql fsql) : base(fsql)
        {
        }

        #region 新增

        /// <summary>
        /// 根据PageGroupModel添加模型。
        /// </summary>
        /// <param name="model">PageGroupModel模型。</param>
        /// <returns>模型的编号。</returns>
        public async Task AddPageGroupModelAsync(BmsPageGroup group)
        {
            await this.InsertAsync(group);
        }
        #endregion

        #region 更新

        /// <summary>
        /// 修改模型。
        /// </summary>
        /// <param name="model">PageGroupModel模型。</param>
        /// <returns></returns>
        public async Task ModifyPageGroupModelAsync(PageGroupModel model)
        {
            await this.UpdateDiy
                       .Where(pg => pg.Id == model.Id)
                       .SetDto(new
                       {
                           model.ParentId,
                           model.Title,
                           model.Icon,
                           model.Sort,
                           model.Status,
                           ModifyTime = DateTime.Now,
                       }).ExecuteAffrowsAsync();
        }
        #endregion

        #region 删除

        /// <summary>
        /// 彻底删除模型。
        /// </summary>
        /// <param name="id">模型的编号。</param>
        /// <returns>异步操作。</returns>
        public async Task RemoveByIdAsync(long id)
        {
            await this.DeleteAsync(pg => pg.Id == id);
        }

        #endregion

        #region 查询

        /// <summary>
        /// 根据id查询是否存在分组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> FindPageAsync(long id)
        {
            return await this.Select.Where(p => p.ParentId == id).AnyAsync();
        }


        /// <summary>
        /// 查询所有模型。
        /// </summary>
        /// <param name="status">状态，为空表示查询所有。</param>
        /// <returns>模型的集合。</returns>
        public Task<List<PageGroupModel>> FindAllAsync(ComStatus? status)
        {
            var query = this.Select;
            if(status.HasValue) query = query.Where(pg => pg.Status == status.Value);

            return query.ToListAsync(pg => new PageGroupModel
            {
                Id = pg.Id,
                ParentId = pg.ParentId,
                Title = pg.Title,
                Icon = pg.Icon,
                Status = pg.Status,
                Sort = pg.Sort,
            });
        }
        #endregion

    }
}
