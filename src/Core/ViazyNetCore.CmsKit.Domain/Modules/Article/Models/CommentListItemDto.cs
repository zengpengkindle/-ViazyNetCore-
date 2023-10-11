using System.ComponentModel.DataAnnotations;

namespace ViazyNetCore.CmsKit.Modules.Models;

public class CommentListItemDto
{
    public long Id { get; set; }
    public long? ParentId { get; set; }

    public long ArticleId { get; set; }

    public long? UserId { get; set; }

    public string UserName { get; set; }

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

    public DateTime? CreateTime { get; set; }
    public DateTime? UpdateTime { get; set; }
}

public class UserCommentListItemDto
{
    public long Id { get; set; }
    public long? ParentId { get; set; }

    public long ArticleId { get; set; }

    public long? UserId { get; set; }

    public string Title { get; set; }

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

    public DateTime? CreateTime { get; set; }
    public DateTime? UpdateTime { get; set; }
    public ArticleType ArticleType { get; set; }
}