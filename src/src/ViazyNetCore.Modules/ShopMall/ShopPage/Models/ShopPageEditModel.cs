using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ViazyNetCore.Modules.ShopMall
{
    public class ShopPageCreateModel
    {
        /// <summary>
        /// 可视化区域编码
        /// </summary>
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string Code { get; set; }
        /// <summary>
        /// 可编辑区域名称
        /// </summary>
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string Description { get; set; }
        /// <summary>
        /// 布局样式编码，1，手机端
        /// </summary>
        public PageLayout Layout { get; set; }
        /// <summary>
        /// 1手机端，2PC端
        /// </summary>
        public PageType Type { get; set; }

        public ComStatus Status { get; set; }
    }

    public class ShopPageEditModel : ShopPageCreateModel
    {
        public long Id { get; set; }
    }
}
