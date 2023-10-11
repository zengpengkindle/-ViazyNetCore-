using System.ComponentModel.DataAnnotations;

namespace ViazyNetCore.CmsKit.Modules.Models;

public class CommentAddDto
{
    public long? ParentId { get; set; }

    public long ArticleId { get; set; }

    public long UserId { get; set; }

    public string Content { get; set; }
}