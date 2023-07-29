using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.TunnelWorks.Models;

namespace ViazyNetCore.TunnelWorks.ManageHost.ViewModels
{
    public class FormTemplateQueryRequest : Pagination
    {
        public string NameLike { get; set; }
        public ComStatus? Status { get; set; }
        /// <summary>
        /// 表单类型
        /// </summary>
        public FormType? FormType { get; set; }
    }
}
