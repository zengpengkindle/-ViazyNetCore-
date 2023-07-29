using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.TunnelWorks.Models;

public enum FormType
{
    /// <summary>
    /// 表单
    /// </summary>
    [Description("表单")]
    Form = 1,
    /// <summary>
    /// 流程
    /// </summary>
    [Description("流程")]
    Flow = 2,
    /// <summary>
    /// 问卷
    /// </summary>
    [Description("问卷")]
    Question = 3
}

public enum FormSourceType
{
    /// <summary>
    /// 默认
    /// </summary>
    [Description("默认")]
    Default = 0,
    /// <summary>
    /// 模板
    /// </summary>
    [Description("模板")]
    Template = 2,
}

public enum FormStatus
{
    [Description("未发布")]
    Create =1,
    [Description("收集中")] 
    Release =2,
    [Description("停止发布")]
    Stop = 3
}