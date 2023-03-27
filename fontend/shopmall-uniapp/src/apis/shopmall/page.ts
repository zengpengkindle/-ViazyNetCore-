import { http } from "@/utils/http";
export class PageApi {
  /**
   * 无
   */
  public getPageData(code?: string): Promise<Array<DesginItem>> {
    return http.request({
      url: "/api/Page/GetPageData",
      method: "post",
      params: { code }
    });
  }
}
export default new PageApi();
/**
 * DesginItem
 */
export interface DesginItem {
  type: string;
  value: any;
}
