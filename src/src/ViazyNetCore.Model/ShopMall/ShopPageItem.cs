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
    [Table(Name = "ShopMall.ShopPageItem")]
    public class ShopPageItem : EntityBase
    {
        /// <summary>
        /// 组件编码
        /// </summary>
        [Display(Name = "组件编码")]
        [Column()]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string WidgetCode { get; set; }
        /// <summary>
        /// 页面编码
        /// </summary>
        [Display(Name = "页面编码")]
        [Column()]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string PageCode { get; set; }
        /// <summary>
        /// 布局位置
        /// </summary>
        [Display(Name = "布局位置")]
        [Column()]
        [Required(ErrorMessage = "请输入{0}")]
        public int PositionId { get; set; }
        /// <summary>
        /// 排序，越小越靠前
        /// </summary>
        [Display(Name = "排序，越小越靠前")]
        [Column()]
        [Required(ErrorMessage = "请输入{0}")]
        public int Sort { get; set; }
        /// <summary>
        /// 组件配置内容
        /// </summary>
        [Display(Name = "组件配置内容")]
        [Column(IsNullable = true)]
        public string Parameters { get; set; }
    }
}
