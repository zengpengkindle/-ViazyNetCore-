namespace ViazyNetCore.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Table(Name = "ShopMall.MemberCar")]
    public partial class MemberCar : EntityBase<string>
    {
        /// <summary>
        /// 
        /// </summary>
        public string MemberId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示购物车内商品种类数量。
        /// </summary>
        public int ItemNum { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示购物车内商品总消费金额。
        /// </summary>
        public decimal TotalMoney { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示购物车内总消费货币1。
        /// </summary>
        public decimal TotalCredit1 { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示购物车内总消费货币2。
        /// </summary>
        public decimal TotalCredit2 { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示购物车内总消费货币3。
        /// </summary>
        public decimal TotalCredit3 { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示购物车内总消费货币4。
        /// </summary>
        public decimal TotalCredit4 { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示购物车内总消费货币5。
        /// </summary>
        public decimal TotalCredit5 { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示创建时间。
        /// </summary>
        public DateTime CreateTime { get; set; }

    }
}
