using FreeSql;
using Microsoft.AspNetCore.Http;

namespace ViazyNetCore.CmsKit.Modules
{
    [Injection]
    public class ArticleCategoryService
    {
        private readonly IBaseRepository<ArticleCategory> _categoryRepogory;
        private readonly IMapper _mapper;

        public ArticleCategoryService(IBaseRepository<ArticleCategory> categoryRepogory
            , IMapper mapper)
        {
            _categoryRepogory = categoryRepogory;
            this._mapper = mapper;
        }

        public async Task<List<ArticleCategoryNode>?> GetNodes()
        {
            var categoryList = await _categoryRepogory.Select
                .IncludeMany(a => a.Posts.Select(p => new Article { Id = p.Id }))
                .ToListAsync();
            return GetNodes(categoryList, 0);
        }

        /// <summary>
        /// 生成文章分类树
        /// </summary>
        private List<ArticleCategoryNode>? GetNodes(List<ArticleCategory> categoryList, int parentId = 0)
        {
            var categories = categoryList
                .Where(a => a.ParentId == parentId && a.Status == ComStatus.Enabled)
                .ToList();

            if (categories.Count == 0) return null;

            return categories.Select(category => new ArticleCategoryNode
            {
                Id = category.Id,
                Text = category.Name,
                Href = "",
                Tags = new List<string> { category.Posts.Count.ToString() },
                Nodes = GetNodes(categoryList, category.Id)
            }).ToList();
        }

        public async Task<List<ArticleCategoryListItemDto>> GetAll(string keyword)
        {
            var result = await _categoryRepogory.Select.OrderBy(a => a.ParentId)
                .WhereIf(keyword.IsNotNull(), c => c.Name.Contains(keyword))
                .ToListAsync();
            return this._mapper.Map<List<ArticleCategory>, List<ArticleCategoryListItemDto>>(result);
        }

        public async Task<PageData<ArticleCategory>> GetPagedList(int page = 1, int pageSize = 10)
        {
            return await _categoryRepogory.Select.ToPageAsync(page, pageSize);
        }

        public async Task<ArticleCategory?> GetById(int id)
        {
            return await _categoryRepogory.Where(a => a.Id == id)
                .Include(a => a.Parent).FirstAsync();
        }

        public async Task<ArticleCategoryDto?> GetDtoById(int id)
        {
            var entity = await _categoryRepogory.Where(a => a.Id == id).FirstAsync();
            return this._mapper.Map<ArticleCategory, ArticleCategoryDto>(entity);
        }


        public async Task<ArticleCategory> AddOrUpdate(ArticleCategoryEditDto dto)
        {
            var entity = this._mapper.Map<ArticleCategoryEditDto, ArticleCategory>(dto);
            return await _categoryRepogory.InsertOrUpdateAsync(entity);
        }

        public async Task<int> Delete(ArticleCategory item)
        {
            return await _categoryRepogory.DeleteAsync(item);
        }

        /// <summary>
        /// 生成分类词云数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<ArticleCatetoryCateWordDto>> GetWordCloud()
        {
            var list = await _categoryRepogory.Select
                .Where(a => a.Status == ComStatus.Enabled && a.ParentId == 0)
                .IncludeMany(a => a.Posts).ToListAsync();

            var data = list.Select(item => new ArticleCatetoryCateWordDto { Id = item.Id, Name = item.Name, Count = item.Posts.Count }).ToList();

            return data;
        }

        public async Task<int> SetStatus(ArticleCategory category, ComStatus status)
        {
            category.Status = status;
            return await _categoryRepogory.UpdateAsync(category);
        }

        /// <summary>
        /// 获取指定分类的层级结构
        /// <para>形式：1,3,5,7,9</para>
        /// </summary>
        public string GetCategoryBreadcrumb(ArticleCategory item)
        {
            var categories = new List<ArticleCategory> { item };
            var parent = item.Parent;
            while (parent != null)
            {
                categories.Add(parent);
                parent = parent.Parent;
            }

            categories.Reverse();
            return string.Join(",", categories.Select(a => a.Id));
        }
    }
}