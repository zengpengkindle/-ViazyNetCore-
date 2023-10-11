using System.Text.RegularExpressions;
using FreeSql;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ViazyNetCore.Authorization.Models;

namespace ViazyNetCore.CmsKit.Modules
{
    [Injection]
    public class CommentService
    {
        private readonly ILogger<CommentService> _logger;
        private readonly IBaseRepository<Comment> _commentRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly IMapper _mapper;

        public CommentService(ILogger<CommentService> logger, IBaseRepository<Comment> commentRepository
            , IMemoryCache memoryCache
            , IMapper mapper)
        {
            _logger = logger;
            _commentRepository = commentRepository;
            _memoryCache = memoryCache;
            this._mapper = mapper;
        }

        private List<Comment>? GetCommentsTree(List<Comment> commentsList, long parentId = 0)
        {
            var comments = commentsList
                .Where(a => a.ParentId == parentId && a.Visible)
                .ToList();

            if (comments.Count == 0) return null;

            return comments.Select(comment =>
            {
                comment.Comments = GetCommentsTree(commentsList, comment.Id);
                return comment;
            }).ToList();
        }

        public async Task<List<Comment>?> GetAll(long articleId)
        {
            var comments = await _commentRepository.Where(a => a.ArticleId == articleId && a.Visible).ToListAsync();
            return GetCommentsTree(comments);
        }

        public async Task<(long total, List<CommentListItemDto>)> GetPagedList(PaginationSort pagination, CommentQueryDto queryDto)
        {
            var querySet = _commentRepository.Select.From<BmsUser>()
                .LeftJoin((c, u) => c.UserId == u.Id)
                .WhereIf(queryDto.UserId > 0, a => a.t1.UserId == queryDto.UserId)
                     .WhereIf(queryDto.OnlyVisible, a => a.t1.Visible)
                     .WhereIf(queryDto.ArticleId > 0, a => a.t1.ArticleId == queryDto.ArticleId)
                     .WhereIf(queryDto.IsNeedAudit != null, a => a.t1.IsNeedAudit == queryDto.IsNeedAudit);

            var result = await querySet.PageSort(out var page, (sortFiled, sort, fsql) =>
            {
                return fsql.Sort(0, p => p.t1.CreateTime);
            }
            , pagination)
            .WithTempQuery(p => new CommentListItemDto
            {
                Id = p.t1.Id,
                UserId = p.t1.UserId,
                ArticleId = p.t1.ArticleId,
                Content = p.t1.Content,
                CreateTime = p.t1.CreateTime,
                IsNeedAudit = p.t1.IsNeedAudit,
                ParentId = p.t1.ParentId,
                Reason = p.t1.Reason,
                UpdateTime = p.t1.UpdateTime,
                UserName = p.t2.Nickname,
                Visible = p.t1.Visible
            }).ToListAsync();
            return (page.Count, result);
        }

        public async Task<(long total, List<UserCommentListItemDto>)> GetUserCommnetPagedList(PaginationSort pagination, CommentQueryDto queryDto)
        {
            var querySet = _commentRepository.Select.From<Article>()
                .LeftJoin((c, u) => c.ArticleId == u.Id)
                .WhereIf(queryDto.UserId > 0, a => a.t1.UserId == queryDto.UserId)
                     .WhereIf(queryDto.OnlyVisible, a => a.t1.Visible)
                     .WhereIf(queryDto.ArticleId > 0, a => a.t1.ArticleId == queryDto.ArticleId)
                     .WhereIf(queryDto.IsNeedAudit != null, a => a.t1.IsNeedAudit == queryDto.IsNeedAudit);

            var result = await querySet.PageSort(out var page, (sortFiled, sort, fsql) =>
            {
                return fsql.Sort(0, p => p.t1.CreateTime);
            }
            , pagination)
            .WithTempQuery(p => new UserCommentListItemDto
            {
                Id = p.t1.Id,
                UserId = p.t1.UserId,
                ArticleId = p.t1.ArticleId,
                Content = p.t1.Content,
                CreateTime = p.t1.CreateTime,
                IsNeedAudit = p.t1.IsNeedAudit,
                ParentId = p.t1.ParentId,
                Reason = p.t1.Reason,
                UpdateTime = p.t1.UpdateTime,
                Title = p.t2.Title,
                ArticleType = p.t2.Type,
                Visible = p.t1.Visible
            }).ToListAsync();
            return (page.Count, result);
        }

        public async Task<Comment?> GetById(long id)
        {
            return await _commentRepository.Where(a => a.Id == id).FirstAsync();
        }

        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public async Task<Comment> Accept(Comment comment, string? reason = null)
        {
            comment.Visible = true;
            comment.IsNeedAudit = false;
            comment.Reason = reason;
            await _commentRepository.UpdateAsync(comment);
            return comment;
        }

        /// <summary>
        /// 审核拒绝
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public async Task<Comment> Reject(Comment comment, string reason)
        {
            comment.Visible = false;
            comment.IsNeedAudit = false;
            comment.Reason = reason;
            await _commentRepository.UpdateAsync(comment);
            return comment;
        }

        public async Task<Comment> Add(CommentAddDto comment)
        {
            var entity = this._mapper.Map<CommentAddDto, Comment>(comment);
            entity.Visible = true;
            entity.IsNeedAudit = false;
            return await _commentRepository.InsertAsync(entity);
        }
    }
}