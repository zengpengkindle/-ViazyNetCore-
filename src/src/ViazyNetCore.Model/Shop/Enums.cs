namespace ViazyNetCore.Model;

/// <summary>
/// 表示购物车内商品状态（0已删除，1购物车内,2已结算）。
/// </summary>
public enum CarItemStatus
{
    /// <summary>
    /// 已删除
    /// </summary>
    Deleted = 0,
    /// <summary>
    /// 购物车内
    /// </summary>
    InCar = 1,
    /// <summary>
    /// 已结算
    /// </summary>
    Ordered = 2
}

/// <summary>
/// 定义商品状态。
/// </summary>
public enum ProductStatus
{
    /// <summary>
    /// 已删除。
    /// </summary>
    Delete = -1,
    /// <summary>
    /// 未上架。
    /// </summary>
    OnStock = 0,
    /// <summary>
    /// 审核中。
    /// </summary>
    Examine = 1,
    /// <summary>
    /// 出售中。
    /// </summary>
    OnSale = 2
}

/// <summary>
/// 定义交易单付款方式。
/// </summary>
public enum PayMode
{
    /// <summary>
    /// 未付款。
    /// </summary>
    UnPay = -1,
    /// <summary>
    /// 微信支付。
    /// </summary>
    Wechat = 0,
    /// <summary>
    /// 支付宝
    /// </summary>
    Alipay = 1,
    /// <summary>
    /// 余额。
    /// </summary>
    Balance = 2,
    /// <summary>
    /// 银联。
    /// </summary>
    Unionpay = 3,
}

/// <summary>
/// 定义申请状态。
/// </summary>
public enum ApplyStatus
{
    /// <summary>
    /// 正在申请。
    /// </summary>
    Apply = 0,
    /// <summary>
    /// 申请成功。
    /// </summary>
    Success = 1,
    /// <summary>
    /// 申请失败。
    /// </summary>
    Fail = 2,
    /// <summary>
    /// 已关闭
    /// </summary>
    Close = -1,
    /// <summary>
    /// 正在代付。
    /// </summary>
    Paying = 3
}

/// <summary>
/// 退换货类型 1.可以退换货，2.只能退货 3.只能换货 4.不能退换货。
/// </summary>
public enum RefundType
{
    /// <summary>
    /// 可以退换货
    /// </summary>
    Support = 1,
    /// <summary>
    /// 只能退货
    /// </summary>
    OnlyReturn = 2,
    /// <summary>
    /// 只能换货
    /// </summary>
    OnlyChange = 3,
    /// <summary>
    /// 不能退换货
    /// </summary>
    NoSupport = 4
}

/// <summary>
/// 定义交易单状态。
/// </summary>
public enum TradeStatus
{

    /// <summary>
    /// 已付款，待提货。
    /// </summary>
    UnPick = -2,
    /// <summary>
    /// 已下单，待付款。
    /// </summary>
    UnPay = -1,
    /// <summary>
    /// 已提货，待发货。
    /// </summary>
    UnDeliver = 0,
    /// <summary>
    /// 已发货，待收货。
    /// </summary>
    UnReceive = 1,
    /// <summary>
    /// 已成功。
    /// </summary>
    Success = 2,
    /// <summary>
    /// 已关闭。
    /// </summary>
    Close = 4
}

public enum TradeRefundStatus
{
    None = 0,
    Applying = 1,

}

/// <summary>
/// 表示订单集合状态（-1已关闭，0未付款，1已付款）。
/// </summary>
public enum TradeSetStatus
{
    /// <summary>
    /// 已关闭
    /// </summary>
    Closed = -1,
    /// <summary>
    /// 未付款
    /// </summary>
    UnPay = 0,
    /// <summary>
    /// 已付款
    /// </summary>
    Success = 1
}

/// <summary>
/// 表示全部退单,部分退单
/// </summary>
public enum ReturnsType
{
    /// <summary>
    /// 全部退单
    /// </summary>
    All = 0,
    /// <summary>
    /// 部分退单
    /// </summary>
    Part = 1
}

/// <summary>
/// 表示退换货处理结果：PUPAWAY:退货入库;REDELIVERY:重新发货;RECLAIM-REDELIVERY:不要求归还并重新发货; REFUND:退款; COMPENSATION:不退货并赔偿。
/// </summary>
public enum HandlingType
{
    PUPAWAY = 1,
    REDELIVERY = 2,
    RECLAIM_REDELIVERY = 3,
    REFUND = 4,
    COMPENSATION = 5
}

/// <summary>
/// 表示处理状态 -1已取消 0申请中 1等待买家退回 2买家已退，卖家等待收货 3卖家已收货，等待处理 4卖家已处理，等待买家确认 5流程结束。
/// </summary>
public enum RefundStatus
{
    /// <summary>
    /// 取消
    /// </summary>
    Cancel = -1,
    /// <summary>
    /// 退换货申请中
    /// </summary>
    Apply = 0,
    /// <summary>
    /// 等待买家退回商品
    /// </summary>
    WaitReturn = 1,
    /// <summary>
    /// 等待卖家收取退回商品
    /// </summary>
    WaitCheck = 2,
    /// <summary>
    /// 卖家已收货，等待处理
    /// </summary>
    Handling = 3,
    /// <summary>
    /// 卖家已处理，等待买家确认
    /// </summary>
    Handed = 4,
    /// <summary>
    /// 退换货处理完毕
    /// </summary>
    Success = 5
}

public enum RefunTradeLogType
{
    Seller = 0,
    Buyer = 1
}

/// <summary>
/// 分销模式（0开放式，1授权式）。
/// </summary>
public enum ShopMode
{
    Opne = 0,
    Authorize = 1
}

public enum FreightType
{
    /// <summary>
    /// 根据商品定价
    /// </summary>
    Product = 0,
    /// <summary>
    /// 根据店铺定价
    /// </summary>
    Shop = 1,
    /// <summary>
    /// 免邮
    /// </summary>
    Free = 2,
    /// <summary>
    /// 阶梯运费
    /// </summary>
    FreightStep = 3,
    /// <summary>
    /// 根据地区计算
    /// </summary>
    FreightArea = 4
}

public enum PayStatus
{
    Paying = 0,
    Success = 1,
    Close = 2,
    Fail = 99
}

public enum RefundTradeStatus
{
    /// <summary>
    /// 未发生
    /// </summary>
    NoHappend = 0,
    /// <summary>
    /// 发起售后
    /// </summary>
    Apply = 1,
    /// <summary>
    /// 已拒绝
    /// </summary>
    Refuse = 2,
    /// <summary>
    /// 处理中
    /// </summary>
    Processing = 3,
    /// <summary>
    /// 已成功
    /// </summary>
    Success = 4
}

public enum Buyway
{
    /// <summary>
    /// 支付宝
    /// </summary>
    AliPay = 10,

    /// <summary>
    /// 微信扫码
    /// </summary>
    WxPay = 20,
}

public enum PayMediaType
{
    /// <summary>
    /// 电脑端
    /// </summary>
    PC = 0,
    /// <summary>
    /// 手机浏览器
    /// </summary>
    WAP = 1,
    /// <summary>
    /// 微信公众号
    /// </summary>
    WeiXinMP = 2,

    /// <summary>
    /// 手机APP端
    /// </summary>
    APP = 3
}
/// <summary>
/// 定义支付结果内容的类型。
/// </summary>
public enum PayContentType
{
    /// <summary>
    /// 表示不显示支付页面，链接直接跳转。
    /// </summary>
    Url = 0,
    /// <summary>
    /// 表示只能使用二维码扫码。
    /// </summary>
    Scan = 1,
    /// <summary>
    /// 表示可扫码，也可以转成链接跳转。
    /// </summary>
    [Obsolete("暂不支持", true)]
    ScanUrl = 2,
    /// <summary>
    /// 表示授权链接，需输入会员密码验证密码方可跳转。
    /// </summary>
    AuthorizeUrl = 3,
    /// <summary>
    /// 表示脚本代码。
    /// </summary>
    Scripts = 4,
    /// <summary>
    /// 表示 H5 支付，如果在微信浏览器当中，必须在浏览器中才可以打开。
    /// </summary>
    H5 = 5,
    /// <summary>
    /// 表示 Html 提交方式。
    /// </summary>
    Html = 6,
    /// <summary>
    /// 表示 HTML 转 URL 提交方式，如果在微信浏览器当中，必须在浏览器中才可以打开。
    /// </summary>
    HtmlUrl = 7,
    /// <summary>
    /// 表示只能使用图片二维码。
    /// </summary>
    ScanImageUrl = 8,
}

public enum ComputeType
{
    /// <summary>
    /// 独立价格
    /// </summary>
    Alone = 0,
    /// <summary>
    /// 与商品设置价格等价
    /// </summary>
    Equal = 1,
    /// <summary>
    /// 与商品等价但计算兑换手续费(固定)
    /// </summary>
    EqualFee = 2,
    /// <summary>
    /// 计算百分比手续费。
    /// </summary>
    EqualFeePercent = 3,
    /// <summary>
    /// 混合价格，即固定价格与现金价格都需要
    /// </summary>
    Hybrid = 4,
    /// <summary>
    /// 条件式，即不作为结算，但购买需要满足该条件才能购买。
    /// </summary>
    Requirement = 5,
    /// <summary>
    /// 赠品
    /// </summary>
    Gift = 6
}

public enum CreditType
{
    /// <summary>
    /// 现金
    /// </summary>
    ReadyMoney = 1,
    /// <summary>
    /// 虚拟货币
    /// </summary>
    Virtual = 2
}

public enum RefundTradeLogType
{
    Seller = 0,
    Buyer = 1
}