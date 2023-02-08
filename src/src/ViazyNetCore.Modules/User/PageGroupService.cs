using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules
{
    /// <summary>
    /// 表示页面分组服务。
    /// </summary>
    [Injection]
    public class PageGroupService : IPageGroupService
    {
        private readonly IPageRepository _pageRepository;
        private readonly IPageGroupRepository _pageGroupRepository;

        public PageGroupService(IPageRepository pageRepository, IPageGroupRepository pageGroupRepository)
        {
            this._pageRepository = pageRepository;
            this._pageGroupRepository = pageGroupRepository;
        }
        /// <summary>
        /// 添加或修改模型。
        /// </summary>
        /// <param name="model">模型。</param>
        /// <returns>模型的编号。</returns>
        public async Task<long> ManageAsync(PageGroupModel model)
        {
            if(model.Id == 0)
            {
                var group = new BmsPageGroup
                {
                    ParentId = model.ParentId,
                    Title = model.Title,
                    Icon = model.Icon,
                    Sort = model.Sort,
                    Status = model.Status,
                    CreateTime = DateTime.Now,
                    ModifyTime = DateTime.Now,
                };
                await _pageGroupRepository.AddPageGroupModelAsync(group);
                model.Id = group.Id;
            }
            else
            {
                await _pageGroupRepository.ModifyPageGroupModelAsync(model);
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
            if(await _pageRepository.FindPageAsync(id)) throw new ApiException("此分组存在页面，请先删除页面。");
            if(await _pageGroupRepository.FindPageAsync(id)) throw new ApiException("此分组存在下级分组，请先删除下级分组。");
            await _pageGroupRepository.RemoveByIdAsync(id);
        }

        /// <summary>
        /// 查询所有模型。
        /// </summary>
        /// <param name="status">状态，为空表示查询所有。</param>
        /// <returns>模型的集合。</returns>
        public Task<List<PageGroupModel>> FindAllAsync(ComStatus? status)
        {
            return _pageGroupRepository.FindAllAsync(status);
        }
    }
}
