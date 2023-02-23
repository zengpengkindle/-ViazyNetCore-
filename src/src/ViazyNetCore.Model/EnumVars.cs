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

}
