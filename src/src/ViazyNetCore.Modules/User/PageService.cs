using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules
{
    /// <summary>
    /// 表示一个页面服务。
    /// </summary>
    [Injection]
    public class PageService : IPageService
    {
        private readonly IPageRepository _pageRepository;

        public PageService(IPageRepository pageRepository)
        {
            this._pageRepository = pageRepository;
        }

        /// <summary>
        /// 添加或修改模型。
        /// </summary>
        /// <param name="model">模型。</param>
        /// <returns>模型的编号。</returns>
        public async Task<long> ManageAsync(PageModel model, long roleId)
        {
            if(model.Id == 0)
            {
                var group = new BmsPage
                {
                    Id = model.Id,
                    GroupId = model.GroupId,
                    Title = model.Title,
                    Icon = model.Icon,
                    Url = model.Url,
                    Sort = model.Sort,
                    Status = model.Status,
                    CreateTime = DateTime.Now,
                    ModifyTime = DateTime.Now,
                };
                await _pageRepository.AddPageGroupModelAsync(group, roleId);
                model.Id = group.Id;
            }
            else
            {
                await _pageRepository.ModifyPageGroupModelAsync(model, roleId);
            }
            return model.Id;
        }

        /// <summary>
        /// 彻底删除模型。
        /// </summary>
        /// <param name="id">模型的编号。</param>
        /// <returns>异步操作。</returns>
        public Task RemoveAsync(long id)
        {
            return _pageRepository.RemoveByIdAsync(id);
        }

        /// <summary>
        /// 查询所有模型。
        /// </summary>
        /// <param name="args">查询参数。</param>
        /// <returns>模型的集合。</returns>
        public Task<PageData<PageFindAllModel>> FindAllAsync(PageFindAllArgs args)
        {
            return _pageRepository.FindAllByPageFindAllArgsAsync(args);
        }

        /// <summary>
        /// 获取基于角色的页面。
        /// </summary>
        /// <param name="roleId">角色编号，空值表示获取所有。</param>
        /// <returns>模型的集合。</returns>
        public Task<List<PageSimpleModel>> GetRolePagesAsync(long roleId)
        {
            return _pageRepository.GetRolePagesAsync(roleId);
        }

    }
}
