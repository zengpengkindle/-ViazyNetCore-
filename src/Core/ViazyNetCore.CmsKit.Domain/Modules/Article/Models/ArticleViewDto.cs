using System.Text.Encodings.Web;
using System.Text.Json;

namespace ViazyNetCore.CmsKit.Modules.Models;

public class ArticleViewDto
{
    public long Id { get; set; }

    public ArticleType Type { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public string Content { get; set; }
    public string ContentHtml { get; set; }
    public string Path { get; set; }
    public string? Url { get; set; }
    public DateTime? CreateTime { get; set; }
    public DateTime? LastUpdateTime { get; set; }
    public ArticleCategory Category { get; set; }
    public List<ArticleCategory> Categories { get; set; }

    public List<string> Images { get; set; }

    public string CreateUserName { get; set; }

}