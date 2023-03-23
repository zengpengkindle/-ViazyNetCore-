import { http } from "@/utils/http";

export class ProductCartApi {
  /**
   * 无
   */
  public findCart(): Promise<ShoppingCart> {
    return http.request({
      url: "/api/ProductCart/FindCart",
      method: "post"
    });
  }
  public addCart(param1?: ShoppingCartProduct): Promise<boolean> {
    return http.request({
      url: "/api/ProductCart/AddCart",
      method: "post",
      data: param1
    });
  }
  /**
   * 无
   */
  public removeCart(param1?: ShoppingCartProduct): Promise<boolean> {
    return http.request({
      url: "/api/ProductCart/RemoveCart",
      method: "post",
      data: param1
    });
  }
}
export default new ProductCartApi();

/**
 * ShoppingCart
 */
export interface ShoppingCart {
  num: number;
  propertyKeys: string;
  propertyValues: string;
  packages: Array<ShoppingCartPackage>;
}

/**
 * ShoppingCartPackage
 */
export interface ShoppingCartPackage {
  shopId: string;
  shopName: string;
  check: boolean;
  items: Array<ShoppingCartProduct>;
}

/**
 * ShoppingCartProduct
 */
export interface ShoppingCartProduct {
  id: string;
  pId: string;
  skuId: string;
  shopId: string;
  pn: string;
  skuText: string;
  imgUrl: string;
  price: number;
  check: boolean;
  num: number;
  status: ComStatus;
}

//
export enum ComStatus {
  Disabled = 0,
  Enabled = 1,
  UnChecked = 2,
  Deleted = -1
}
export interface ShoppingCartProduct {
  id: string | null;
  pId: string | null;
  skuId: string | null;
  shopId: string | null;
  pn: string | null;
  skuText: string | null;
  imgUrl: string | null;
  price: number | null;
  check: boolean | null;
  num: number | null;
  status: ComStatus | null;
}
