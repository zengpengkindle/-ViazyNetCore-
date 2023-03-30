using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ViazyNetCore.Modules.ShopMall
{
    public class ProductModel
    {
        #region 商品基本属性
        /// <summary>
        /// 设置或获取一个值，表示主键。
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示品牌编号。
        /// </summary>
        public string BrandId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示品牌名称。
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示类目编号。
        /// </summary>
        public string CatId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示类目名称。
        /// </summary>
        public string CatName { get; set; }

        /// <summary>
        /// 商家编号
        /// </summary>
        public string ShopId { get; set; }

        /// <summary>
        /// 商家名称
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示类目路径。
        /// </summary>
        public string CatPath { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示副标题。
        /// </summary>
        public string SubTitle { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示关键词。
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示描述。
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示成本。
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示最低售价。
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示针对普通订单，是否包邮。
        /// </summary>
        public bool IsFreeFreight { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示运费。
        /// </summary>
        public decimal Freight { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示运费阶梯数，运费计算方式为：“订单商品数量/运费阶梯数”的值，如果存在小数数据则四舍五入取大值。
        /// </summary>
        public int FreightStep { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示运费阶梯值。
        /// </summary>
        public decimal FreightValue { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示是否在前台隐藏。
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示状态变更时间。
        /// </summary>
        public DateTime StatusChangeTime { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示主图。
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示辅图，最多4张，用逗号分隔。
        /// </summary>
        public string SubImage { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示是否开启属性。
        /// </summary>
        public bool OpenSpec { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示详情。
        /// </summary>
        public string Detail { get; set; } = "";

        public ProductStatus Status { get; set; }

        public RefundType RefundType { get; set; }
        #endregion

        public StockModel Stock { get; set; } = new StockModel();

        #region 基本属性
        /// <summary>
        /// 设置或获取一个值，表示创建时间。
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示修改时间。
        /// </summary>
        public DateTime ModifyTime { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示商品搜索内容，格式（商品标题+"_"+商品副标题+"_"+品牌名称+"_"+类目路径）。
        /// </summary>
        public string SearchContent { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示是否有外部关联。
        /// </summary>
        public bool HasOuter { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示外部关联类型，多类型使用,隔开。
        /// </summary>
        public string OuterType { get; set; }



        /// <summary>
        /// 设置或获取一个值，表示扩展数据。
        /// </summary>
        public string Exdata { get; set; }

        public string PropertyKeys { get; set; }

        public string PropertyValues { get; set; }
        #endregion

        public ProductSkuModel Skus { get; set; } = new ProductSkuModel();
    }

    public class ProductSkuModel
    {
        public List<SkuTree> Tree { get; set; } = new List<SkuTree>();

        public List<SkuModel> List { get; set; } = new List<SkuModel>();

        public decimal Price { get; set; }

        [JsonProperty("stock_num")]
        public int StockNum { get; set; }

        [JsonProperty("collection_id")]
        public string CollectionId { get; set; }

        [JsonProperty("none_sku")]
        public bool NoneSku { get; set; }

        [JsonProperty("hide_stock")]
        public bool HideStock { get; set; } = false;

        public Dictionary<string, decimal> SpecialPrices { get; set; }
    }

    public class ProductInfoModel
    {
        /// <summary>
        /// 设置或获取一个值，表示商品编号。
        /// </summary>
        public string ProductId { get; set; }
        /// <summary>
        /// 设置或获取一个值，表示商家编号。
        /// </summary>
        public string ShopId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示商家名称。
        /// </summary>
        public string ShopName { get; set; }

        public string CatId { get; set; }

        public string CatName { get; set; }

        public string BrandId { get; set; }


        public string BrandName { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示商品标题。
        /// </summary>
        public string Title { get; set; }

        public string SubTitle { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示商品图片。
        /// </summary>
        public string Image { get; set; }

        public string SubImage { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示详情。
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SkuId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示是否有外部关联。
        /// </summary>
        public bool HasOuter { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示外部关联类型，多类型使用,隔开。
        /// </summary>
        public string OuterType { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示商品规格名称组合文本,如"颜色:红色 尺码:32"。
        /// </summary>
        public string SkuText { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示成本价。
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// 当前商品剩余库存
        /// </summary>
        public int InStock { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示 退换货类型 1.可以退换货，2.只能退货 3.只能换货 4.不能退换货。
        /// </summary>
        public RefundType RefundType { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示商品售价。
        /// </summary>
        public decimal Price { get; set; }

        public ProductStatus Status { get; set; }

        public FreightType FreightType { get; set; }

        public decimal FreightValue { get; set; }
        public decimal FreightStep { get; set; }

        public decimal FreightStepValue { get; set; }

        public int Num { get; set; }

        public ProductSkuModel Skus { get; set; }
    }

    public class SkuTree
    {
        public string K { get; set; }

        public List<SkuTreeValue> V { get; set; } = new List<SkuTreeValue>();

        [JsonProperty("k_s")]
        public string KeyValue { get; set; }
    }

    public class SkuTreeValue
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ImgUrl { get; set; }
    }


    public class SkuModel
    {
        public string Id { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示 规格类目 k_s 为 s1 的对应规格值 id
        /// </summary>
        public string S1 { get; set; }

        /// <summary>
        /// 规格s1名称 如：颜色
        /// </summary>
        public string Key1 { get; set; }

        /// <summary>
        /// 规格s1名称 如：颜色
        /// </summary>
        public string Name1 { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示 规格类目 k_s 为 s2 的对应规格值 id 为0表示不存在该规格
        /// </summary>
        public string S2 { get; set; }

        /// <summary>
        /// 规格s2名称 如：颜色
        /// </summary>
        public string Key2 { get; set; }

        /// <summary>
        /// 规格s2值 如：红色
        /// </summary>
        public string Name2 { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示 规格类目 k_s 为 s3 的对应规格值 id 为0表示不存在该规格
        /// </summary>
        public string S3 { get; set; }

        /// <summary>
        /// 规格s3名称 如：颜色
        /// </summary>
        public string Key3 { get; set; }

        /// <summary>
        /// 规格s3名称 如：颜色
        /// </summary>
        public string Name3 { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示售价。
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示售价。
        /// </summary>
        public decimal Price { get; set; }
        [JsonProperty("stock_num")]
        public int StockNum { get; set; }

        /// <summary>
        /// 特殊定价
        /// </summary>
        public Dictionary<string, decimal> SpecialPrices { get; set; }
    }

    public class FindAllArguments : Pagination
    {
        public string? ShopId { get; set; }

        public string? TitleLike { get; set; }

        public string? CatName { get; set; }

        public bool? IsHidden { get; set; }

        public ProductStatus? Status { get; set; }

        public DateTime[] CreateTimes { get; set; }
        public string CatId { get; set; }
    }
}
