namespace ViazyNetCore.Model
{
    /// <summary>
    /// 表示一个商品。
    /// </summary>
    [Table(Name = "ShopMall.Product")]
    public partial class Product : EntityBase<string>
    {
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
        /// 设置或获取一个值，表示商品状态（-1已删除，0未上架，1出售中）。
        /// </summary>
        public ProductStatus Status { get; set; }

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
        /// 商品规格属性 JSON 字符串，如“[{k: '颜色',v: [{id: '30349',name: '红色',imgUrl: '1.jpg'},{id: '1215',name: '蓝色',imgUrl: 2.jpg'}],k_s: 's1'}]”
        /// </summary>
        public string SkuTree { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示详情。
        /// </summary>
        public string Detail { get; set; }

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
        /// 设置或获取一个值，表示 退换货类型 1.可以退换货，2.只能退货 3.只能换货 4.不能退换货。
        /// </summary>
        public RefundType RefundType { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示扩展数据。
        /// </summary>
        public string Exdata { get; set; }

    }

}
