import { http } from "@/utils/http";

export class ProductCatApi {
  /**
   * 新增或编辑商品分类
   */
  public edit(param1?: CatEditReq): Promise<void> {
    return http.request({
      url: "/api/ProductCat/Edit",
      method: "post",
      data: param1
    });
  }
  /**
   * 根据Id查找类目
   */
  public get(id?: string): Promise<CatRes> {
    return http.request({
      url: "/api/ProductCat/Get",
      method: "post",
      params: { id }
    });
  }
  /**
   * 搜索分类分页查询
   */
  public findPageList(
    sort?: number,
    sortField?: string,
    page?: number,
    limit?: number
  ): Promise<CatResPageData> {
    return http.request({
      url: "/api/ProductCat/FindPageList",
      method: "post",
      params: { Sort: sort, SortField: sortField, Page: page, Limit: limit }
    });
  }
}
export default new ProductCatApi();

/**
 * CatEditReq
 */
export interface CatEditReq {
  id: string | null;
  /** 设置或获取一个值，表示是否在前台隐藏。 */ isHidden: boolean | null;
  /** 设置或获取一个值，表示名称。 */ name: string | null;
  /** 设置或获取一个值，表示上级编号。 */ parentId: string | null;
  /** 设置或获取一个值，表示是否父级。 */ isParent: boolean | null;
  /** 设置或获取一个值，表示类目路径。 */ path: string | null;
  /** 设置或获取一个值，表示图片。 */ image: string | null;
  /** 设置或获取一个值，表示排序。 */ sort: number | null;
  status: ComStatus | null;
  /** 设置或获取一个值，表示扩展数据。 */ exdata: string | null;
}

//
export enum ComStatus {
  Disabled = 0,
  Enabled = 1,
  UnChecked = 2,
  Deleted = -1
}

/**
 * CatResPageData
 */
export interface CatResPageData {
  rows: Array<CatRes>;
  total: number;
}

/**
 * CatRes
 */
export interface CatRes {
  id: string;
  /** 设置或获取一个值，表示是否在前台隐藏。 */ isHidden: boolean;
  /** 设置或获取一个值，表示名称。 */ name: string;
  /** 设置或获取一个值，表示上级编号。 */ parentId: string;
  /** 设置或获取一个值，表示是否父级。 */ isParent: boolean;
  /** 设置或获取一个值，表示类目路径。 */ path: string;
  /** 设置或获取一个值，表示图片。 */ image: string;
  /** 设置或获取一个值，表示排序。 */ sort: number;
  status: ComStatus;
  /** 设置或获取一个值，表示扩展数据。 */ exdata: string;
}
