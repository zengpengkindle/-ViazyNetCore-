using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ViazyNetCore.Model
{
    public class AdvertPosition : EntityBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "名称")]
        [StringLength(120, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string Name { get; set; }
        /// <summary>
        /// 位置编码
        /// </summary>
        [Display(Name = "位置编码")]
        [StringLength(32, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string Code { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        [Display(Name = "添加时间")]
        public System.DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        public System.DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        [Display(Name = "是否启用")]
        [Required(ErrorMessage = "请输入{0}")]
        public ComStatus Status { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public int Sort { get; set; }
    }
}
