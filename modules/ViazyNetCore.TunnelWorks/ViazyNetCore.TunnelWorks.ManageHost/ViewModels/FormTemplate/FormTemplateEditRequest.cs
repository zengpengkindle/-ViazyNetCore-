using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.TunnelWorks.Models;

namespace ViazyNetCore.TunnelWorks.ManageHost.ViewModels
{
    public class FormTemplateEditRequest
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
        /// 表单类型
        /// </summary>
        public FormType FormType { get; set; }
    }
}
