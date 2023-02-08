using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules
{
    /// <summary>
    /// 表示一个页面服务仓储。
    /// </summary>
    [Injection]
    public class PageRepository : DefaultRepository<BmsPage, long>, IPageRepository
    {
        private const string BMS_PAGE_ROLE_PAGE_ID = "BMS_PAGE_ROLE_PAGE_ID_{0}";
        private readonly IRolePageRepository _rolePageRepository;
        private readonly ICacheService _cacheService;

        public PageRepository(IFreeSql fsql, ICacheService cacheService, IRolePageRepository rolePageRepository) : base(fsql)
        {
            this._cacheService = cacheService;
            this._rolePageRepository = rolePageRepository;
        }

        #region 新增

        /// <summary>
        /// 根据PageModel添加模型。
        /// </summary>
        /// <param name="model">PageModel模型。</param>
        /// <returns>模型的编号。</returns>
        public async Task AddPageGroupModelAsync(BmsPage group, long roleId)
        {
            RemovePageCacheByPageId(group.Id);
            RemovePageCacheByRoleId(roleId);
            await this.InsertAsync(group);
        }

        #endregion

        #region 更新

        /// <summary>
        /// 修改模型。
        /// </summary>
        /// <param name="model">PageModel模型。</param>
        /// <returns></returns>
        public async Task ModifyPageGroupModelAsync(PageModel model, long roleId)
        {
            RemovePageCacheByPageId(model.Id);
            RemovePageCacheByRoleId(roleId);
            await this.UpdateDiy
                  .Where(pg => pg.Id == model.Id)
                  .SetDto(new
                  {
                      model.GroupId,
                      model.Title,
                      model.Icon,
                      model.Url,
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
        public Task RemoveByIdAsync(long id)
        {
            RemovePageCacheByPageId(id);
            _cacheService.Remove(string.Format(BMS_PAGE_ROLE_PAGE_ID, id));
            return this.DeleteAsync(p => p.Id == id);
        }

        #endregion

        #region 查询

        /// <summary>
        /// 根据PageFindAllArgs查询所有模型。
        /// </summary>
        /// <param name="args">查询参数。</param>
        /// <returns>模型的集合。</returns>
        public Task<PageData<PageFindAllModel>> FindAllByPageFindAllArgsAsync(PageFindAllArgs args)
        {
            var query = this.Select;
            if(args.GroupId > 0) query = query.Where(p => p.GroupId == args.GroupId);
            if(args.TitleLike.IsNotNull()) query = query.Where(p => p.Title.Contains(args.TitleLike));
            if(args.Status.HasValue) query = query.Where(p => p.Status == args.Status.Value);
            else query = query.Where(a => a.Status != ComStatus.Deleted);

            return query
                .OrderBy(p => p.Sort)
                .WithTempQuery(p => new PageFindAllModel
                {
                    Id = p.Id,
                    GroupId = p.GroupId,
                    Title = p.Title,
                    Icon = p.Icon,
                    Url = p.Url,
                    Sort = p.Sort,
                    Status = p.Status,
                    CreateTime = p.CreateTime,
                    ModifyTime = p.ModifyTime,
                }).ToPageAsync(args);
        }

        /// <summary>
        /// 获取基于角色的页面。
        /// </summary>
        /// <param name="roleId">角色编号，空值表示获取所有。</param>
        /// <returns>模型的集合。</returns>
        public Task<List<PageSimpleModel>> GetRolePagesAsync(long roleId)
        {
            return _cacheService.GetAsync(string.Format(BMS_PAGE_ROLE_PAGE_ID, roleId), () =>
             {
                 var pages = this.Select;

                 if(roleId > 0 && roleId != Globals.ADMIN_ROLE_ID)
                 {
                     pages = this.Select.Where(p => this.Orm.Select<BmsRolePage>().Where(rp => rp.RoleId == roleId && p.Id == rp.PageId).Any());
                 }

                 return pages.Where(p => p.Status == ComStatus.Enabled).WithTempQuery(p => new PageSimpleModel
                 {
                     Id = p.Id,
                     GroupId = p.GroupId,
                     Title = p.Title,
                     Icon = p.Icon,
                     Url = p.Url,
                     Sort = p.Sort
                 }).ToListAsync();
             });
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public void RemovePageCacheByRoleId(long roleId)
        {
            var key = string.Format(BMS_PAGE_ROLE_PAGE_ID, roleId);
            _cacheService.Remove(key);
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public void RemovePageCacheByPageId(long pageId)
        {
            var roleIdList = _rolePageRepository.Select.Where(o => o.PageId == pageId).WithTempQuery(o => o.RoleId).ToList();
            foreach(var roleId in roleIdList)
            {
                var key = string.Format(BMS_PAGE_ROLE_PAGE_ID, roleId);
                _cacheService.Remove(key);
            }
            _cacheService.Remove(string.Format(BMS_PAGE_ROLE_PAGE_ID, Globals.ADMIN_ROLE_ID));
        }


        /// <summary>
        /// 根据id获取是否存在页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> FindPageAsync(long id)
        {
            return this.Select.Where(p => p.GroupId == id).AnyAsync();
        }

        #endregion

    }
}
