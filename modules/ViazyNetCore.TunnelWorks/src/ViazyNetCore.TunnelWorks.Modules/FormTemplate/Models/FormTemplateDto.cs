using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.TunnelWorks.Models;

namespace ViazyNetCore.TunnelWorks.Modules.Models;

public class FormTemplateDto
{
    public long Id { get; set; }
    /// <summary>
    /// 表单名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 表单来源
    /// </summary>
    public FormSourceType SourceType { get; set; }
    /// <summary>
    /// 来源Id
    /// </summary>
    public long SourceId { get; set; }

    public FormStatus PublichStatus { get; set; }

    public ComStatus Status { get; set; }

    /// <summary>
    /// 表单类型
    /// </summary>
    public FormType FormType { get; set; }
}

public class FormTemplateEditDto : FormTemplateDto
{
}

public class FormTemplateListQueryDto
{
    public string NameLike { get; set; }
    public ComStatus? Status { get; set; }
    /// <summary>
    /// 表单类型
    /// </summary>
    public FormType? FormType { get; set; }
}