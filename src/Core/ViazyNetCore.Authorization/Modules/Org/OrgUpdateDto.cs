using System.ComponentModel.DataAnnotations;
namespace ViazyNetCore.Authorization;

/// <summary>
/// 修改
/// </summary>
public class OrgUpdateDto : OrgAddDto
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required]
    public long Id { get; set; }
}