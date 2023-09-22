import { http } from "@/utils/http";
import { ComStatus } from "../model";
/**
 * SpecialCreditPagination
 */
export interface SpecialCreditPagination {
  outerType: string | null;

  page: number | null;

  limit: number | null;
}

/**
 * ProductOuterSpecialCreditPageData
 */
export interface ProductOuterSpecialCreditPageData {
  rows: Array<ProductOuterSpecialCredit>;

  total: number;
}

/**
 * ProductOuterSpecialCredit
 */
export interface ProductOuterSpecialCredit {
  outerType: string;

  creditKey: string;

  objectKey: string;

  objectName: string;

  computeType: ComputeType;

  status: ComStatus;

  feeMoney: number;

  feePercent: number;

  exdata: string;

  createTime: string;

  id: string;
}

/**
 * OuterKeySpecialCredit
 */
export interface OuterKeySpecialCredit {
  key: string;

  name: string;

  creditKey: string;

  computeType: ComputeType;

  feeMoney: number;

  feePercent: number;
}
//
export enum ComputeType {
  Alone = 0,
  Equal = 1,
  EqualFee = 2,
  EqualFeePercent = 3,
  Hybrid = 4,
  Requirement = 5,
  Gift = 6
}
export class ProductOuterSpecialCreditApi {
  /**
   * 无
   */
  public findAll(
    param1?: SpecialCreditPagination
  ): Promise<ProductOuterSpecialCreditPageData> {
    return http.request({
      url: "/api/ProductOuterSpecialCredit/FindAll",
      method: "post",
      data: param1
    });
  }
  public get(id?: string): Promise<ProductOuterSpecialCredit> {
    return http.request({
      url: "/api/ProductOuterSpecialCredit/Get",
      method: "post",
      params: { id }
    });
  }
  public modifyStatus(outerId?: string, status?: ComStatus): Promise<boolean> {
    return http.request({
      url: "/api/ProductOuterSpecialCredit/ModifyStatus",
      method: "post",
      params: { outerId, status }
    });
  }
  public manger(param1?: ProductOuterSpecialCredit): Promise<void> {
    return http.request({
      url: "/api/ProductOuterSpecialCredit/Manger",
      method: "post",
      data: param1
    });
  }
  public getSpecialCreditByOuterKey(
    outerType?: string
  ): Promise<Array<OuterKeySpecialCredit>> {
    return http.request({
      url: "/api/ProductOuterSpecialCredit/GetSpecialCreditByOuterKey",
      method: "post",
      params: { outerType }
    });
  }
}
export default new ProductOuterSpecialCreditApi();
