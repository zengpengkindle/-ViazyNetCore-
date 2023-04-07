using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Model
{
    /// <summary>
    /// 表示一个商品Spec。
    /// </summary>
    [Table(Name = "ShopMall.ProductSpec")]
    public class ProductSpec : EntityBase<string>
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 销售属性  eg:颜色
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否销售属性
        /// </summary>
        public int IsLeaf { get; set; }

        /// <summary>
        /// 通用状态
        /// </summary>
        public ComStatus Status { get; set; }

        /// <summary>
        /// 销售属性排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column(CanUpdate = false)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
