import { http } from "@/utils/http";
/**
 * VehicleQueryRequest
 */
export interface VehicleQueryRequest {
  /** 分类 */ catId: number | null;
  /** 机构 */ orgId: number | null;
  /** 单件装备编码 */ code: string | null;
  /** 产地国别 */ originPlace: string | null;
  /** 车牌号 */ licenseCode: string | null;
  /** 车架号码 */ frameCode: string | null;
  /** 发动机号 */ engineCode: string | null;
}

/**
 * VehicleListItemResultPageData
 */
export interface VehicleListItemResultPageData {
  rows: Array<VehicleListItemResult>;
  total: number;
}

/**
 * VehicleListItemResult
 */
export interface VehicleListItemResult {
  id: number;
  /** 单件装备编码 */ code: string;
  /** 机构编码 */ orgId: number;
  orgName: string;
  vehicleStatus: VehicleStatus;
  /** 装备分类 */ catId: number;
  /** 分类名称 */ catName: string;
  /** 规格型号 */ spec: string;
  /** 生产企业 */ originEnterprise: string;
  /** 产地国别 */ originPlace: string;
  /** 车牌号 */ licenseCode: string;
  /** 车架号码 */ frameCode: string;
  /** 发动机号 */ engineCode: string;
  createTime: string;
}

//
export enum VehicleStatus {
  /** 新品 */ New = 1
}
/**
 * VehicleEditRequest
 */
export interface VehicleEditRequest {
  id: number | null;
  /** 单件装备编码 */ code: string | null;
  /** 车辆照片 */ vehicleImg: string | null;
  /** 机构编码 */ orgId: number | null;
  vehicleStatus: VehicleStatus | null;
  /** 装备分类 */ catId: number | null;
  /** 规格型号 */ spec: string | null;
  /** 生产企业 */ originEnterprise: string | null;
  /** 产地国别 */ originPlace: string | null;
  /** 车牌号 */ licenseCode: string | null;
  /** 车架号码 */ frameCode: string | null;
  /** 发动机号 */ engineCode: string | null;
}
export class VehicleApi {
  /**
   * 无
   */
  public list(
    query: {
      sort?: number;
      sortField?: string;
      page?: number;
      limit?: number;
    },
    param1?: VehicleQueryRequest
  ): Promise<VehicleListItemResultPageData> {
    return http.request({
      url: "/api/vehicle/list",
      method: "post",
      data: param1,
      params: {
        Sort: query.sort,
        SortField: query.sortField,
        Page: query.page,
        Limit: query.limit
      }
    });
  }
  public edit(param1?: VehicleEditRequest): Promise<void> {
    return http.request({
      url: "/api/vehicle/edit",
      method: "post",
      data: param1
    });
  }
  public getInfo(id: number): Promise<VehicleEditRequest> {
    return http.request({
      url: "/api/vehicle/getInfo",
      method: "post",
      params: { id }
    });
  }
}
export default new VehicleApi();
