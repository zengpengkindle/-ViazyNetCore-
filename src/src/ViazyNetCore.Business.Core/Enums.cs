using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore
{
    /// <summary>
    /// 微信消息类型[关联CoreCmsWeixinMessage表type字段]
    /// </summary>
    public enum WeiChatMessageTypes
    {
        [Description("文本消息")]
        文本消息 = 1,
        [Description("图片消息")]
        图片消息 = 2,
        [Description("图文消息")]
        图文消息 = 3
    }

    /// <summary>
    /// 微信支付交易类型
    /// </summary>
    public enum WeiChatPayTradeType
    {
        [Description("JSAPI")]
        JSAPI = 1,
        [Description("JSAPI_OFFICIAL")]
        JSAPI_OFFICIAL = 2,
        [Description("NATIVE")]
        NATIVE = 3,
        [Description("APP")]
        APP = 4,
        [Description("MWEB")]
        MWEB = 5
    }
}
