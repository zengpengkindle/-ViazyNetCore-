using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ViazyNetCore.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Table(Name = "ShopMall.LogisticsCompany")]
    public partial class LogisticsCompany : EntityBase<string>
    {
        /// <summary>
        /// 设置或获取一个值，表示物流公司代码。
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示物流公司简称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示运单号验证正则表达式。
        /// </summary>
        public string RegMailNo { get; set; }

    }
}
