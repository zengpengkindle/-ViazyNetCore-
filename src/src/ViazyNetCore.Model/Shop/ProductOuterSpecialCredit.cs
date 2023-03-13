namespace ViazyNetCore.Model
{
    [Table(Name = "ShopMall.ProductOuterSpecialCredit")]
    public class ProductOuterSpecialCredit : EntityBase<string>
    {

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
        /// 设置或获取一个值，表示价格类型名称
        /// </summary>
        public string ObjectName { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示价格计算方式
        /// </summary>
        public ComputeType ComputeType { get; set; }

        public ComStatus Status { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示价格计算额外费用
        /// </summary>
        public decimal? FeeMoney { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示价格计算额外费用百分比
        /// </summary>
        public decimal? FeePercent { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示扩展数据
        /// </summary>
        public string Exdata { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
