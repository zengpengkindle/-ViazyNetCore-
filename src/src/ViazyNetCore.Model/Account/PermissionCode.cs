using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ViazyNetCore
{
    /**
     * 命名规则
     * 枚举名称:功能_操作,例如:Merchant_Withraw_Enabled 商户提现单 开启开关功能
     * PermissionCode_BMS,枚举以 1_0000 开始
     * PermissionCode_Merchant,枚举以 2_0000 开始
     * PermissionCode_Agent,枚举以 3_0000 开始
     */
    /// <summary>
    /// BMS
    /// </summary>
    public enum BMSPermissionCode
    {
        [Description("所有权限")]
        All
    }

    /// <summary>
    /// Merchant
    /// </summary>
    public enum MerchantPermissionCode
    {
        [Description("所有权限")]
        All = 2_0000,
    }

    /// <summary>
    /// Agent
    /// </summary>
    public enum AgentPermissionCode
    {
        [Description("所有权限")]
        All = 3_0000,
    }

}