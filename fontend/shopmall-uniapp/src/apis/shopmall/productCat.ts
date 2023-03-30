import { http } from "@/utils/http";

export class ProductCatApi {
  /**
   * 无
   */
  public getCats(): Promise<Array<ProductCat>> {
    return http.request({
      url: "/api/ProductCat/GetCats",
      method: "post"
    });
  }
}
export default new ProductCatApi();
/**
 * ProductCat
 */
export interface ProductCat {
  isHidden: boolean;
  name: string;
  parentId: string;
  isParent: boolean;
  path: string;
  image: string;
  sort: number;
  status: ComStatus;
  exdata: string;
  id: string;
}

//
export enum ComStatus {
  Disabled = 0,
  Enabled = 1,
  UnChecked = 2,
  Deleted = -1
}
