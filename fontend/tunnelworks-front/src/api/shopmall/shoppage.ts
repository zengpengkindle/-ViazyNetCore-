import { http } from "@/utils/http";

export class PageApi {
  /**
   * 无
   */
  public getPageList(param1?: ShopPageQuery): Promise<ShopPagePageData> {
    return http.request({
      url: "/api/shopmall/GetPageList",
      method: "post",
      data: param1
    });
  }
  public updatePage(param1?: ShopPageEditModel): Promise<void> {
    return http.request({
      url: "/api/shopmall/UpdatePage",
      method: "post",
      data: param1
    });
  }
  public deletePage(id?: number): Promise<void> {
    return http.request({
      url: "/api/shopmall/DeletePage",
      method: "post",
      params: { id }
    });
  }
  public savePageDesign(param1?: PageDesignEditRes): Promise<void> {
    return http.request({
      url: "/api/shopmall/SavePageDesign",
      method: "post",
      data: param1
    });
  }
  public getPageData(id?: number): Promise<PageDesignEditRes> {
    return http.request({
      url: "/api/shopmall/GetPageData",
      method: "post",
      params: { id }
    });
  }
}
/**
 * PageDesignEditRes
 */
export interface PageDesignEditRes {
  code: string | null;
  items: Array<DesginItem> | null;
}

/**
 * DesginItem
 */
export interface DesginItem {
  key?: string;
  type: string;
  value: any;
}

/**
 * ShopPageQuery
 */
export interface ShopPageQuery {
  code: string | null;
  name: string | null;
  layout: PageLayout | null;
  type: PageType | null;
  page: number | null;
  limit: number | null;
}

/**
 * ShopPagePageData
 */
export interface ShopPagePageData {
  rows: Array<ShopPage>;
  total: number;
}

/**
 * ShopPage
 */
export interface ShopPage {
  code: string;
  name: string;
  description: string;
  layout: PageLayout;
  type: PageType;
  status: ComStatus;
  createTime: string;
  modifyTime: string;
  id: number;
}

//
export enum PageLayout {
  Mobile = 1,
  Pc = 2
}
//
export enum PageType {
  Mobile = 1,
  Pc = 2
}
//
export enum ComStatus {
  Disabled = 0,
  Enabled = 1,
  UnChecked = 2,
  Deleted = -1
}
/**
 * ShopPageEditModel
 */
export interface ShopPageEditModel {
  id: number | null;
  code: string | null;
  name: string | null;
  description: string | null;
  layout: PageLayout | null;
  type: PageType | null;
  status: ComStatus | null;
}
export default new PageApi();
