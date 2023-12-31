﻿using System.ComponentModel.DataAnnotations;

namespace ViazyNetCore.CmsKit
{
    public class DictionaryTypeAddInput
    {
        /// <summary>
        /// 字典名称
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        /// <summary>
        /// 字典编码
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Code { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        public ComStatus Status { get; set; }
    }

    public class DictionaryTypeUpdateInput : DictionaryTypeAddInput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Required]
        public long Id { get; set; }
    }
}
