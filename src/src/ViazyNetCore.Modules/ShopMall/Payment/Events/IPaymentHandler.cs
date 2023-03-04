using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.ShopMall
{
    public interface IPaymentHandler
    {
        Buyway Buyway { get; }
        Task<PayCreateValue> CreatePayment(PayMediaType payMediaType,TradeViewModel trade);
    }


    public class TradeViewModel
    {
        /// <summary>
        /// 商户网站唯一订单号
        /// </summary>
        public string OutTradeNo { get; set; }

        public string MemberId { get; set; }
        /// <summary>
        /// 商品的标题/交易标题/订单标题/订单关键字等。
        /// </summary>
        public string Subject { get; set; }
        ///// <summary>
        ///// 销售产品码，商家和支付宝签约的产品码
        ///// </summary>
        //public string ProductCode { get;internal set; }
        /// <summary>
        /// 对一笔交易的具体描述信息。如果是多种商品，请将商品描述字符串累加传给body。
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 订单总金额，单位为元，精确到小数点后两位，取值范围[0.01,100000000]
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 前台回调地址
        /// </summary>
        public string ReturnUrl { get; set; }
    }
}
