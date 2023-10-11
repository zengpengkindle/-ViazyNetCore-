using FreeSql.DataAnnotations;

namespace ViazyNetCore.CmsKit.Models;

/// <summary>
/// 文章评论
/// </summary>
public class Comment : EntityUpdate
{
    public long ParentId { get; set; }

    public Comment? Parent { get; set; }

    public List<Comment>? Comments { get; set; }

    public long ArticleId { get; set; }

    public Article Article { get; set; }

    public long? UserId { get; set; }

    public string? UserAgent { get; set; }

    public string Content { get; set; }
    public bool Visible { get; set; }

    /// <summary>
    /// 是否需要审核
    /// </summary>
    public bool IsNeedAudit { get; set; } = false;

    /// <summary>
    /// 原因
    /// <para>如果验证不通过的话，可能会附上原因</para>
    /// </summary>
    public string? Reason { get; set; }
}