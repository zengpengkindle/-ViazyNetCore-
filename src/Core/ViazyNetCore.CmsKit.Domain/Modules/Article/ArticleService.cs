using FreeSql;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ViazyNetCore.CmsKit.Modules
{
    [Injection]
    public class ArticleService
    {
        private readonly ILogger<ArticleService> _logger;
        private readonly IMapper _mapper;
        private readonly IBaseRepository<Article> _articleRepository;
        private readonly IBaseRepository<ArticleCategory> _categoryRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _accessor;


        public ArticleService(
            IMapper mapper,
            IBaseRepository<Article> articleRepo,
            IBaseRepository<ArticleCategory> categoryRepo,
            IWebHostEnvironment environment,
            IHttpContextAccessor accessor,
            ILogger<ArticleService> logger)
        {
            this._mapper = mapper;
            _articleRepository = articleRepo;
            _categoryRepository = categoryRepo;
            _environment = environment;
            _accessor = accessor;
            _logger = logger;
        }

        public async Task<Article?> GetById(long id)
        {
            // 获取文章的时候对markdown中的图片地址解析，加上完整地址返回给前端
            var article = await _articleRepository.Where(a => a.Id == id).Include(a => a.Category).FirstAsync();
            return article;
        }

        public async Task<ArticleInfoDto?> GetDtoById(long id)
        {
            // 获取文章的时候对markdown中的图片地址解析，加上完整地址返回给前端
            var article = await _articleRepository.Where(a => a.Id == id).FirstAsync();
            if (article != null)
                return this._mapper.Map<Article, ArticleInfoDto>(article);
            return null;
        }

        public async Task<int> Delete(long id)
        {
            return await _articleRepository.DeleteAsync(a => a.Id == id);
        }

        public async Task<Article> UpsertAsync(ArticleEditDto editDto)
        {
            var article = this._mapper.Map<ArticleEditDto, Article>(editDto);
            var articleId = article.Id;
            // 是新文章的话，先保存到数据库
            if (await _articleRepository.Where(a => a.Id == articleId).CountAsync() == 0)
            {
                article = await _articleRepository.InsertAsync(article);
            }

            // 检查文章中的外部图片，下载并进行替换
            // todo 将外部图片下载放到异步任务中执行，以免保存文章的时候太慢
            // article.Content = await MdExternalUrlDownloadAsync(article);
            //// 修改文章时，将markdown中的图片地址替换成相对路径再保存
            //article.Content = MdImageLinkConvert(article, false);

            // 处理完内容再更新一次
            await _articleRepository.UpdateAsync(article);
            return article;
        }


        public async Task<PageData<Article>> GetPagedList(ArticleQueryDto queryDto, PaginationSort pagination)
        {
            var querySet = _articleRepository.Select;

            // 是否发布
            if (queryDto.OnlyPublished)
            {
                querySet = _articleRepository.Select.Where(a => a.IsPublish);
            }

            // 状态过滤
            if (queryDto.Status != null)
            {
                querySet = querySet.Where(a => a.Status == queryDto.Status);
            }

            // 分类过滤
            if (queryDto.CategoryId != 0)
            {
                querySet = querySet.Where(a => a.CategoryId == queryDto.CategoryId);
            }
            querySet = querySet.WhereIf(queryDto.Search.IsNotNull(), a => a.Title.Contains(queryDto.Search))
                .WhereIf(queryDto.Type != null, a => a.Type == queryDto.Type);

            var result = await querySet.Include(a => a.Category)
                .PageSort(out var page, (sortFiled, sort, fsql) =>
                {
                    return fsql.Sort(0,p=>p.CreateTime);
                }, pagination)
                .ToListAsync();
            return PageData.Create(result, page.Count);
        }

        public async Task<ArticleViewDto> GetArticleViewById(long id)
        {
            var article = await GetById(id);
            if (article == null)
                throw new ApiException("博文不存在或已被删除");

            var result = await this.GetArticleViewModel(article);
            return result;
        }

        /// <summary>
        /// 将 Article 对象转换为 ArticleViewModel 对象
        /// </summary>
        public async Task<ArticleViewDto> GetArticleViewModel(Article article)
        {
            var model = new ArticleViewDto
            {
                Id = article.Id,
                Title = article.Title,
                Summary = article.Summary ?? "（没有介绍）",
                Content = article.Content ?? "（没有内容）",
                Path = article.Path ?? string.Empty,
                Url = "",
                Type = article.Type,
                CreateTime = article.CreateTime,
                CreateUserName = article.CreateUserName,
                LastUpdateTime = article.UpdateTime,
                Category = article.Category,
                Categories = new List<ArticleCategory>()
            };

            if (!string.IsNullOrWhiteSpace(article.Slug))
            {
                model.Url = "";
            }

            if (article.Categories != null)
            {
                foreach (var itemId in article.Categories.Split(",").Select(int.Parse))
                {
                    var item = await _categoryRepository.Where(a => a.Id == itemId).FirstAsync();
                    if (item != null) model.Categories.Add(item);
                }
            }

            return model;
        }


        /// <summary>
        /// 初始化博客文章的资源目录
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        private string InitArticleMediaDir(Article article)
        {
            var blogMediaDir = Path.Combine(_environment.WebRootPath, "media", "blog");
            var articleMediaDir = Path.Combine(_environment.WebRootPath, "media", "blog", article.Id.ToString());
            if (!Directory.Exists(blogMediaDir)) Directory.CreateDirectory(blogMediaDir);
            if (!Directory.Exists(articleMediaDir)) Directory.CreateDirectory(articleMediaDir);

            return articleMediaDir;
        }
    }
}