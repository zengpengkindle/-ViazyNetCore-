using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Model
{


    #region redis缓存类型
    public enum AccessTokenEnum
    {

        /// <summary>
        /// 微信小程序
        /// </summary>
        WxOpenAccessToken = 1,

        /// <summary>
        /// 微信公众号
        /// </summary>
        WeiXinAccessToken = 2,

        /// <summary>
        /// 易联云打印机
        /// </summary>
        YiLianYunAccessToken = 3,
    }
    #endregion

    public enum UserAccountTypes
    {
        [Description("微信公众号")]
        WxMp = 1,
        [Description("微信小程序")]
        WxApp = 2,
        [Description("支付宝小程序")]
        AliApp = 3,
        [Description("微信APP快捷登陆")]
        WxQuick = 4,
        [Description("QQ在APP中快捷登陆")]
        QQH5 = 5,
        [Description("头条系小程序")]
        TouTiaoApp = 6,
    }

    public enum OperatorTypeEnum
    {
        Bms = 1
    }

    /// <summary>
    /// 表示日志级别。
    /// </summary>
    public enum LogRecordLevel
    {
        /// <summary>
        /// 表示跟踪级别。
        /// </summary>
        Trace = 0,
        /// <summary>
        /// 表示调试级别。
        /// </summary>
        Debug = 1,
        /// <summary>
        /// 表示信息级别。
        /// </summary>
        Information = 2,
        /// <summary>
        /// 表示警告级别。
        /// </summary>
        Warning = 3,
        /// <summary>
        /// 表示错误级别。
        /// </summary>
        Error = 4,
        /// <summary>
        /// 表示严重错误级别。
        /// </summary>
        Critical = 5,
        /// <summary>
        /// 表示无级别。
        /// </summary>
        None = 6
    }
}
