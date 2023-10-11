using FreeSql.DataAnnotations;

namespace ViazyNetCore.CmsKit.Models;

/// <summary>
/// 文章分类
/// </summary>
public class ArticleCategory : EntityAdd<int>
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public override int Id { get; set; }

    public string Name { get; set; }

    public int ParentId { get; set; }
    public ArticleCategory? Parent { get; set; }

    /// <summary>
    /// 分类是否可见
    /// </summary>
    public ComStatus Status { get; set; }

    public List<Article> Posts { get; set; }
}