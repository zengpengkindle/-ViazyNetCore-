using System.ComponentModel.DataAnnotations;
using FreeSql.DataAnnotations;

namespace ViazyNetCore.CmsKit.Models;

/// <summary>
/// 文章管理
/// </summary>
public class Article : EntityUpdate
{
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }

    [Column(MapType = typeof(int))]
    public ArticleType Type { get; set; }

    /// <summary>
    /// 文章封面链接，设置后可以通过以下形式访问文章
    /// </summary>
    [MaxLength(500)]
    public string? Slug { get; set; }

    /// <summary>
    /// 文章状态
    /// </summary>
    [Column(MapType = typeof(int))]
    public ComStatus Status { get; set; }

    /// <summary>
    /// 是否发表（不发表的话就是草稿状态）
    /// </summary>
    public bool IsPublish { get; set; }

    /// <summary>
    /// 梗概
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    /// 内容（markdown格式）
    /// </summary>
    [MaxLength(-1)]
    public string? Content { get; set; }

    /// <summary>
    /// 博客在导入前的相对路径
    /// <para>如："系列/AspNetCore开发笔记"</para>
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// 分类ID
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// 分类
    /// </summary>
    public ArticleCategory? Category { get; set; }

    /// <summary>
    /// 文章的分类层级, 其内容类似这样 `1,2,3` , 用逗号分隔开分类ID
    /// </summary>
    public string? Categories { get; set; }
}

public enum ArticleType
{
    Article = 1,
    Notify = 2
}