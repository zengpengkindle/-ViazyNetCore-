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
    [Table(Name = "ShopMall.ShopPage")]
    public class ShopPage : EntityBase
    {
        /// <summary>
        /// 可视化区域编码
        /// </summary>
        [Display(Name = "可视化区域编码")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string Code { get; set; }
        /// <summary>
        /// 可编辑区域名称
        /// </summary>
        [Display(Name = "可编辑区域名称")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "描述")]
        [Column(IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string Description { get; set; }
        /// <summary>
        /// 布局样式编码，1，手机端
        /// </summary>
        [Display(Name = "布局样式编码，1，手机端")]
        [Column(IsNullable = true)]
        public PageLayout? Layout { get; set; }
        /// <summary>
        /// 1手机端，2PC端
        /// </summary>
        [Display(Name = "1手机端，2PC端")]
        [Column(IsNullable = true)]
        public PageType? Type { get; set; }

        public ComStatus Status { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示创建时间。
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示修改时间。
        /// </summary>
        public DateTime ModifyTime { get; set; }

    }
}
