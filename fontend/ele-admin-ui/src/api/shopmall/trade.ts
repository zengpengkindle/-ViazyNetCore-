import { http } from "@/utils/http";
import { Pagination } from "../model";

export class TradeApi {
  public findWlList(): Promise<Array<SimpleLogisticsCompany>> {
    return http.request({
      url: "/api/Trade/FindWlList",
      method: "post"
    });
  }
  public findTrade(tradeId?: string): Promise<TradeDetailModel> {
    return http.request({
      url: "/api/Trade/FindTrade",
      method: "post",
      params: { tradeId }
    });
  }
  /**
   * 无
   */
  public findAll(
    param1?: TradePageArgments
  ): Promise<TradeDetailModelPageData> {
    return http.request({
      url: "/api/Trade/FindAll",
      method: "post",
      data: param1
    });
  }
  public deliverTrades(param1?: BatchDeliveryModel): Promise<DeliverFail> {
    return http.request({
      url: "/api/Trade/DeliverTrades",
      method: "post",
      data: param1
    });
  }
  public changeTradeDeliver(
    tradeId?: string,
    param1?: DeliveryModel
  ): Promise<void> {
    return http.request({
      url: "/api/Trade/ChangeTradeDeliver",
      method: "post",
      data: param1,
      params: { tradeId }
    });
  }
  public changeTradeAddressModel(
    param1?: ChangeTradeAddressModel
  ): Promise<void> {
    return http.request({
      url: "/api/Trade/ChangeTradeAddressModel",
      method: "post",
      data: param1
    });
  }
}
export default new TradeApi();
export interface SimpleLogisticsCompany {
  id: string;
  name: string;
  code: string;
}
export interface ChangeTradeAddressModel {
  id: string | null;
  receiverName: string | null;
  receiverMobile: string | null;
  receiverProvince: string | null;
  receiverCity: string | null;
  receiverDistrict: string | null;
  receiverDetail: string | null;
}
/**
 * TradePageArgments
 */
export interface TradePageArgments extends Pagination {
  memberId: string | null;
  tradeId: string | null;
  nickNameLike: string | null;
  username: string | null;
  shopId: string | null;
  shopName: string | null;
  createTimes: Array<string> | null;
  begin: string | null;
  end: string | null;
  tradeStatus: TradeStatus | null;
  timeType: number | null;
  payMode: PayMode | null;
  pageNumber: number | null;
  pageSize: number | null;
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
  totalFreight: number;
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
 * BatchDeliveryModel
 */
export interface BatchDeliveryModel {
  tradeIds: Array<string> | null;
  delivery: DeliveryModel | null;
}

/**
 * DeliverFail
 */
export interface DeliverFail {
  value: any;
  fail: number;
  failIds: Array<string>;
}

/**
 * DeliveryModel
 */
export interface DeliveryModel {
  logisticsId: string;
  logisticsFee: number;
  logisticsCode: string;
  logisticsCompany: string;
  address: AddressModel;
}

/**
 * AddressModel
 */
export interface AddressModel {
  id: string;
  province: string;
  city: string;
  county: string;
  addressDetail: string;
  postalCode: string;
  areaCode: string;
  name: string;
  address: string;
  tel: string;
  isDefault: boolean;
}
