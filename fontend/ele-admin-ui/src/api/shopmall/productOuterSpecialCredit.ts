import { http } from "@/utils/http";
import { ComStatus } from "../model";
/**
 * SpecialCreditPagination
 */
export interface SpecialCreditPagination {
  outerType?: string | null;

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
  public apiProductOuterSpecialCreditFindAll(
    param1?: SpecialCreditPagination
  ): Promise<ProductOuterSpecialCreditPageData> {
    return http.request({
      url: "/api/ProductOuterSpecialCredit/FindAll",
      method: "post",
      data: param1
    });
  }
  public apiProductOuterSpecialCreditModifyStatus(
    outerId?: string,
    status?: ComStatus
  ): Promise<boolean> {
    return http.request({
      url: "/api/ProductOuterSpecialCredit/ModifyStatus",
      method: "post",
      params: { outerId, status }
    });
  }
}
export default new ProductOuterSpecialCreditApi();
