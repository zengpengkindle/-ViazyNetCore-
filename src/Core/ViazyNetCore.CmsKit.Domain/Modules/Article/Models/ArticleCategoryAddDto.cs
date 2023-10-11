namespace ViazyNetCore.CmsKit.Modules.Models;

public record ArticleCategoryAddDto
{
    public string Name { get; set; }
    public int ParentId { get; set; }
    public bool Visible { get; set; } = true;
}