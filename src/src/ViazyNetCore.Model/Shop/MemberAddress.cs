namespace ViazyNetCore.Model
{
    /// <summary>
    /// 表示一个会员收货地址。
    /// </summary>
    public partial class MemberAddress : EntityBase<string>
    {
        /// <summary>
        /// 设置或获取一个值，表示会员编号。
        /// </summary>
        public string MemberId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示是否默认。
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示收货地址所在的省份。
        /// </summary>
        public string ReceiverProvince { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示收货地址所在的城市。
        /// </summary>
        public string ReceiverCity { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示收货地址所在的区县。
        /// </summary>
        public string ReceiverDistrict { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示详细的收货地址。
        /// </summary>
        public string ReceiverDetail { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示收货人姓名。
        /// </summary>
        public string ReceiverName { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示收货人手机。
        /// </summary>
        public string ReceiverMobile { get; set; }

        /// <summary>
        /// 地区编码 前端控件使用
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        public string PostalCode { get; set; }

    }
}
