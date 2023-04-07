using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ViazyNetCore.Modules.ShopMall.Models
{
    public class ProductSkuDto
    {
        public string Key { get; set; }
        public string Text { get; set; }
    }
    public class ProductSpecTreeDto
    {
        public string Key { get; set; }
        public string Text { get; set; }

        public List<ProductSkuDto> Value { get; set; }
    }

    public class ProductSkuPriceDto
    {
        public string SkuId { get; set; }
        /// <summary>
        /// 售价
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 成本价
        /// </summary>
        public decimal CostPrice { get; set; }
        public string Code { get; set; }

        /// <summary>
        /// 销售属性值，规格值的组合： [红色{key},尺码41{key}]
        /// </summary>
        [JsonIgnore]
        public string SpecDetailIdsStr { get; set; }
        /// <summary>
        /// 销售属性值，规格值的组合： [红色{key},尺码41{key}]
        /// </summary>
        public List<string>? SpecDetailList
        {
            get => this.SpecDetailIdsStr == null ? null : JsonConvert.DeserializeObject<List<string>>(this.SpecDetailIdsStr);
        }

        /// <summary>
        /// 销售属性名
        /// </summary>
        public string SpecDetailText { get; set; }
    }

    public class ProductSpecTreePrice
    {
        public List<ProductSpecTreeDto> Specs { get; set; }
        public List<ProductSkuPriceDto> SpecPrices { get; set; }
    }
}
