import { http } from "@/utils/http";
import { ComStatus } from "../model";

/**
 * Pagination
 */
export interface Pagination {
  page: number | null;

  limit: number | null;
}

/**
 * CreditsPageData
 */
export interface CreditsPageData {
  rows: Array<Credits>;

  total: number;
}

/**
 * Credits
 */
export interface Credits {
  id: string;

  creditType: CreditType;

  status: ComStatus;

  name: string;

  creditKey: string;

  exdata: string;

  createTime: string;
}
export interface CreditModel {
  name: string | null;

  key: string | null;

  creditType: CreditType | null;
}
//
export enum CreditType {
  ReadyMoney = 1,
  Virtual = 2
}

export class CreditApi {
  /**
   * 无
   */
  public apiCreditFindAll(param1?: Pagination): Promise<CreditsPageData> {
    return http.request({
      url: "/api/Credit/FindAll",
      method: "post",
      data: param1
    });
  }
  public apiCreditModifyStatus(
    id?: string,
    status?: ComStatus
  ): Promise<boolean> {
    return http.request({
      url: "/api/Credit/ModifyStatus",
      method: "post",
      params: { id, status }
    });
  }
  public apiCreditAddCredit(param1?: CreditModel): Promise<boolean> {
    return http.request({
      url: "/api/Credit/AddCredit",
      method: "post",
      data: param1
    });
  }
  public getAll(): any {
    return http.request({
      url: "/api/Credit/GetAll",
      method: "post"
    });
  }
}
export default new CreditApi();
