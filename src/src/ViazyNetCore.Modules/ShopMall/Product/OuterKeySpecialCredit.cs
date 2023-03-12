using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.ShopMall
{
    public class OuterKeySpecialCredit
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public string CreditKey { get; set; }

        public ComputeType ComputeType { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示价格计算额外费用
        /// </summary>
        public decimal? FeeMoney { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示价格计算额外费用百分比
        /// </summary>
        public decimal? FeePercent { get; set; }
    }

    public class SpecialCreditPagination : Pagination
    {
        public string OuterType { get; set; }
    }
}
