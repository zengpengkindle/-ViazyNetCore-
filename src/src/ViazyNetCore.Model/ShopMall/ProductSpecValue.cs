using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Model.ShopMall
{
    [Table(Name = "ShopMall.ProductSpecValue")]
    public class ProductSpecValue : EntityBase<string>
    {
        /// <summary>
        /// 规格ID
        /// </summary>
        public long SpecId { get; set; }

        /// <summary>
        /// 销售属性
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否是销售属性值
        /// </summary>
        public int IsLeaf { get; set; }

        /// <summary>
        /// 在线状态
        /// </summary>
        public ProductStatus Status { get; set; }

        /// <summary>
        /// 规格图片uri
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 商品Id
        /// </summary>
        public string ProductId { get; set; }

        [Column(CanUpdate = false)]
        public long CreateTime { get; set; }

        public long UpdateTime { get; set; }
    }
}
