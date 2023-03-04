using System;
using System.Collections.Generic;
using System.Text;

namespace ViazyNetCore.Modules.ShopMall
{
    public class AlipayMiddlewareOptions
    {
        public string Host { get; set; }
        /// <summary>
        /// 设置或获取一个地址，表示支付宝支付回调地址
        /// </summary>
        public string NotifyUrl { get; set; } = "alipay/notify";
        public string AppNotifyRoute { get; set; } = "apppay";
        public string PageNotifyRoute { get; set; } = "pagepay";
        public string WapNotifyRoute { get; set; } = "wappay";
    }
}
