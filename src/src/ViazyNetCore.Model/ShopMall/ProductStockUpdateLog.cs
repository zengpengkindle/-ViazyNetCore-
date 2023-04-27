namespace ViazyNetCore.Model
{
    /// <summary>
    /// 商品在库库存变更记录（只记录后台主动修改数值）
    /// </summary>
    [Table(Name = "ShopMall.ProductStockUpdateLog")]
    public partial class ProductStockUpdateLog : EntityBase<string>
    {
        /// <summary>
        /// 库存记录编号
        /// </summary>
        public string StockId { get; set; }

        /// <summary>
        /// 在库库存旧数值
        /// </summary>
        public int OldInStock { get; set; }

        /// <summary>
        /// 在库库存新数值
        /// </summary>
        public int NewInStock { get; set; }

        /// <summary>
        /// 在库库存变动数值
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 记录产生时间
        /// </summary>
        public DateTime CreateTime { get; set; }

    }
}
