namespace ViazyNetCore.CmsKit.Modules.Models;

public class ArticleCategoryNode {
    public int Id { get; set; }
    public string? Text { get; set; }
    public string? Href { get; set; }
    public List<string> Tags { get; set; }
    public List<ArticleCategoryNode>? Nodes { get; set; }
}