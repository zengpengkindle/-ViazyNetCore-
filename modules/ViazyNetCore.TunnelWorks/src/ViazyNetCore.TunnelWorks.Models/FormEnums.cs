using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.TunnelWorks.Models
{
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
}
