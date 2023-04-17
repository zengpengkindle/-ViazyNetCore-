using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.ShopMall
{
    /// <summary>
    /// 表示一个支付创建的结果。
    /// </summary>
    public class PayCreateValue
    {
        /// <summary>
        /// 获取或设置一个值，表示一个支付内容。
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示通道主体订单号。
        /// </summary>
        public string BodyOrderId { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示支付内容的类型。
        /// </summary>
        public PayContentType ContentType { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示支付结果页面，为空表示默认页面。
        /// </summary>
        public string ContentPage { get; set; }
    }
}
