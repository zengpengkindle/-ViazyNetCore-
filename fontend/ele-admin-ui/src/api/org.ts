import { http } from "@/utils/http";
import { ComStatus } from "./model";
/**
 * OrgGetOutput
 */
export interface OrgGetOutput {
  id: number;

  parentId: number;

  name: string;

  code: string;

  value: string;

  status: ComStatus;

  sort: number;

  description: string;
}

/**
 * OrgListOutput
 */
export interface OrgListOutput {
  id: number;

  parentId: number;

  name: string;

  code: string;

  value: string;

  status: ComStatus;

  sort: number;

  description: string;

  createdTime: string;
}
/**
 * OrgAddInput
 */
export interface OrgAddInput {
  parentId: number | null;

  name: string | null;

  code: string | null;

  value: string | null;

  status: ComStatus | null;

  sort: number | null;

  description: string | null;
}
export interface OrgUpdateInput {
  id: number | null;

  parentId: number | null;

  name: string | null;

  code: string | null;

  value: string | null;

  status: ComStatus | null;

  sort: number | null;

  description: string | null;
}
export class OrgApi {
  /**
   * 无
   */
  public apiOrgGet(id?: number): Promise<OrgGetOutput> {
    return http.request({
      url: "/api/Org/get",
      method: "post",
      params: { id }
    });
  }
  public apiOrgGetList(key?: string): Promise<Array<OrgListOutput>> {
    return http.request({
      url: "/api/Org/getList",
      method: "post",
      params: { key }
    });
  }
  public apiOrgAdd(param1?: OrgAddInput): Promise<number> {
    return http.request({
      url: "/api/Org/add",
      method: "post",
      data: param1
    });
  }

  public apiOrgUpdate(param1?: OrgUpdateInput): Promise<boolean> {
    return http.request({
      url: "/api/Org/update",
      method: "post",
      data: param1
    });
  }
  public apiOrgDelete(id?: number): Promise<boolean> {
    return http.request({
      url: "/api/Org/delete",
      method: "post",
      params: { id }
    });
  }
}
export default new OrgApi();
