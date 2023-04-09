import { http } from "@/utils/http";

export class TradeApi {
  /**
   * 无
   */
  public beforeTrade(
    param1?: Array<BeforeTradeItem>
  ): Promise<CreateTradeSetModel> {
    return http.request({
      url: "/api/Trade/BeforeTrade",
      method: "post",
      data: param1
    });
  }

  public createTrade(param1?: Array<BeforeTradeItem>): Promise<Array<string>> {
    return http.request({
      url: "/api/Trade/CreateTrade",
      method: "post",
      data: param1
    });
  }

  public bindTrade(param1?: CreateTradeSetModel): Promise<Array<string>> {
    return http.request({
      url: "/api/Trade/BindTrade",
      method: "post",
      data: param1
    });
  }
  /**
   * 无
   */
  public findMyTrades(
    param1?: TradePageReq
  ): Promise<TradeDetailModelPageData> {
    return http.request({
      url: "/api/Trade/FindMyTrades",
      method: "post",
      data: param1
    });
  }
  public findTrade(tradeId?: string): Promise<TradeDetailModel> {
    return http.request({
      url: "/api/Trade/FindTrade",
      method: "post",
      params: { tradeId }
    });
  }
  public findTradesPayInfo(param1?: Array<string>): Promise<TradeSetModel> {
    return http.request({
      url: "/api/Trade/FindTradesPayInfo",
      method: "post",
      data: param1
    });
  }
}
export default new TradeApi();

/**
 * BeforeTradeItem
 */
export interface BeforeTradeItem {
  productId: string | null;
  skuId: string | null;
  price: number | null;
  num: number | null;
}

/**
 * CreateTradeSetModel
 */
export interface CreateTradeSetModel {
  totalMoney: number;
  num: number;
  addressId: string;
  shopTrades: Array<ShopTrade>;
}

/**
 * StringObjectKeyValuePair
 */
export interface StringObjectKeyValuePair {
  key: string;
  value: any;
}

/**
 * ShopTrade
 */
export interface ShopTrade {
  shopId: string;
  shopName: string;
  items: Array<TradeItem>;
  message: string;
  voucherId: string;
  productMoney: number;
  totalMoney: number;
  totalFreight: number;
}

/**
 * TradeItem
 */
export interface TradeItem {
  pId: string;
  pn: string;
  skuId: string;
  skuText: string;
  price: number;
  num: number;
  imgUrl: string;
  refundType: RefundType;
}

//
export enum RefundType {
  Support = 1,
  OnlyReturn = 2,
  OnlyChange = 3,
  NoSupport = 4
}

/**
 * CreateTradeSetModel
 */
export interface CreateTradeSetModel {
  totalMoney: number | null;
  addressId: string | null;
  shopTrades: Array<ShopTrade> | null;
  properties: Array<StringObjectKeyValuePair> | null;
}

/**
 * ShopTrade
 */
export interface ShopTrade {
  shopId: string;
  shopName: string;
  items: Array<TradeItem>;
  message: string;
  voucherId: string;
  productMoney: number;
  totalMoney: number;
  totalFreight: number;
}

/**
 * TradeItem
 */
export interface TradeItem {
  pId: string;
  pn: string;
  skuId: string;
  skuText: string;
  price: number;
  num: number;
  imgUrl: string;
  refundType: RefundType;
  properties: Array<StringObjectKeyValuePair>;
}

/**
 * StringObjectKeyValuePair
 */
export interface StringObjectKeyValuePair {
  key: string;
  value: any;
}
/**
 * TradePageArgments
 */
export interface TradePageReq {
  tradeStatus: TradeStatus | null;
  page: number | null;
  limit: number | null;
}

/**
 * TradeDetailModelPageData
 */
export interface TradeDetailModelPageData {
  rows: Array<TradeDetailModel>;
  total: number;
}

/**
 * TradeDetailModel
 */
export interface TradeDetailModel {
  id: string;
  shopId: string;
  shopName: string;
  userName: string;
  name: string;
  tradeStatus: TradeStatus;
  payMode: PayMode;
  createTime: string;
  payTime: string;
  consignTime: string;
  completeTime: string;
  statusChangedTime: string;
  totalfeight: number;
  totalMoney: number;
  productMoney: number;
  message: string;
  count: number;
  items: Array<TradeOrderModel>;
  address: TradeAddress;
  receiverProvince: string;
  receiverCity: string;
  receiverDistrict: string;
  receiverDetail: string;
  receiverName: string;
  receiverMobile: string;
  logistics: TradeLogistics;
  logisticsId: string;
  logisticsCode: string;
  logisticsCompany: string;
  logisticsFee: number;
}

/**
 * TradeOrderModel
 */
export interface TradeOrderModel {
  orderId: string;
  imgUrl: string;
  pn: string;
  price: number;
  pId: string;
  skuId: string;
  skuText: string;
  num: number;
}

/**
 * TradeAddress
 */
export interface TradeAddress {
  id: string;
  tel: string;
  address: string;
}

/**
 * TradeLogistics
 */
export interface TradeLogistics {
  name: string;
  expressNo: string;
  createTime: string;
}

//
export enum TradeStatus {
  UnDeliver = 0,
  UnReceive = 1,
  Success = 2,
  Close = 4,
  UnPick = -2,
  UnPay = -1
}
//
export enum PayMode {
  Wechat = 0,
  Alipay = 1,
  Balance = 2,
  Unionpay = 3,
  UnPay = -1
}

/**
 * TradeDetailModel
 */
export interface TradeDetailModel {
  id: string;
  shopId: string;
  shopName: string;
  userName: string;
  name: string;
  tradeStatus: TradeStatus;
  payMode: PayMode;
  createTime: string;
  payTime: string;
  consignTime: string;
  completeTime: string;
  statusChangedTime: string;
  totalfeight: number;
  totalMoney: number;
  productMoney: number;
  message: string;
  count: number;
  items: Array<TradeOrderModel>;
  address: TradeAddress;
  receiverProvince: string;
  receiverCity: string;
  receiverDistrict: string;
  receiverDetail: string;
  receiverName: string;
  receiverMobile: string;
  logistics: TradeLogistics;
  logisticsId: string;
  logisticsCode: string;
  logisticsCompany: string;
  logisticsFee: number;
}

/**
 * TradeOrderModel
 */
export interface TradeOrderModel {
  orderId: string;
  imgUrl: string;
  pn: string;
  price: number;
  pId: string;
  skuId: string;
  skuText: string;
  num: number;
}

/**
 * TradeAddress
 */
export interface TradeAddress {
  id: string;
  tel: string;
  address: string;
}

/**
 * TradeLogistics
 */
export interface TradeLogistics {
  name: string;
  expressNo: string;
  createTime: string;
}

/**
 * TradeSetModel
 */
export interface TradeSetModel {
  trades: Array<TradeDetailModel>;
  totalProductMoney: number;
  totalFreigh: number;
  totalMoney: number;
  totalDiscount: number;
  createTime: string;
  noUnPayCount: number;
  addressFailCount: number;
  address: TradeAddress;
}

/**
 * TradeDetailModel
 */
export interface TradeDetailModel {
  id: string;
  shopId: string;
  shopName: string;
  userName: string;
  name: string;
  tradeStatus: TradeStatus;
  payMode: PayMode;
  createTime: string;
  payTime: string;
  consignTime: string;
  completeTime: string;
  statusChangedTime: string;
  totalfeight: number;
  totalMoney: number;
  productMoney: number;
  message: string;
  count: number;
  items: Array<TradeOrderModel>;
  address: TradeAddress;
  receiverProvince: string;
  receiverCity: string;
  receiverDistrict: string;
  receiverDetail: string;
  receiverName: string;
  receiverMobile: string;
  logistics: TradeLogistics;
  logisticsId: string;
  logisticsCode: string;
  logisticsCompany: string;
  logisticsFee: number;
}

/**
 * TradeOrderModel
 */
export interface TradeOrderModel {
  orderId: string;
  imgUrl: string;
  pn: string;
  price: number;
  pId: string;
  skuId: string;
  skuText: string;
  num: number;
}

/**
 * TradeAddress
 */
export interface TradeAddress {
  id: string;
  tel: string;
  address: string;
}

/**
 * TradeLogistics
 */
export interface TradeLogistics {
  name: string;
  expressNo: string;
  createTime: string;
}
