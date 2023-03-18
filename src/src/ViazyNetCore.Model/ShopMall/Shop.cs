namespace ViazyNetCore.Model
{
    /// <summary>
    /// 表示一个供应商表。
    /// </summary>
    [Table(Name = "ShopMall.Shop")]
    public partial class Shop : EntityBase<string>
    {
        /// <summary>
        /// 设置或获取一个值，表示公司名称。
        /// </summary>
        public string ComponyName { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示联系人姓名。
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示店铺电话。
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示发货地址。
        /// </summary>
        public string AddressFH { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示退货地址。
        /// </summary>
        public string AddressTH { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示分销模式（0开放式，1授权式）。
        /// </summary>
        public ShopMode OpenMode { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示公告简介。
        /// </summary>
        public string Intro { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示店铺标识。
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示店铺横幅。
        /// </summary>
        public string Banner { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示营业时间。
        /// </summary>
        public string OfficeTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal MinDistribution { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Area { get; set; }

    }
}
