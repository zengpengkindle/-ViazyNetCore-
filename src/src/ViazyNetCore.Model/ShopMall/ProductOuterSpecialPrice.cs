using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Model
{
    [Table(Name = "ShopMall.ProductOuterSpecialPrice")]
    public class ProductOuterSpecialPrice : EntityBase<string>
    {
        /// <summary>
        /// 设置或获取一个值，表示商品编号。
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示SKU编码。
        /// </summary>
        public string SkuId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示外部关联类型。
        /// </summary>
        public string OuterType { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示货币类型标识Key
        /// </summary>
        public string CreditKey { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示价格类型标识
        /// </summary>
        public string ObjectKey { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示售价。
        /// </summary>
        public decimal Price { get; set; }
    }
}
