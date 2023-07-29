﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.TunnelWorks.Models
{
    /// <summary>
    /// 表单
    /// </summary>
    public class FormTemplate : EntityUpdate
    {
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
}
