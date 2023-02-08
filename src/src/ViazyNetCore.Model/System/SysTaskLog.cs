using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ViazyNetCore.Model
{
    /// <summary>
    /// 定时任务日志
    /// </summary>
    public class SysTaskLog
    {
        /// <summary>
        /// 序列
        /// </summary>
        [Display(Name = "序列")]
        [Column( IsPrimary = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public long Id { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        [Display(Name = "任务名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string Name { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        [Required(ErrorMessage = "请输入{0}")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 是否完成
        /// </summary>
        [Required(ErrorMessage = "请输入{0}")]
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 其他数据
        /// </summary>
        [Display(Name = "其他数据")]
        public string Parameters { get; set; }
    }
}
