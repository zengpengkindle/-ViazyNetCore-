import { http } from "@/utils/http";

export class ProductApi {
  /**
   * 无
   */
  public getProductInfo(productId?: string): Promise<ProductInfoModel> {
    return http.request({
      url: "/api/Product/GetProductInfo",
      method: "post",
      params: { productId }
    });
  }
  public getProductSku(
    productId: string,
    outerType?: string
  ): Promise<ProductInfoModel> {
    return http.request({
      url: "/api/Product/GetProductSku",
      method: "post",
      params: { productId, outerType }
    });
  }
}
export default new ProductApi();
/**
 * ProductInfoModel
 */
export interface ProductInfoModel {
  productId: string;
  shopId: string;
  shopName: string;
  catId: string;
  catName: string;
  brandId: string;
  brandName: string;
  title: string;
  subTitle: string;
  image: string;
  subImage: string;
  detail: string;
  skuId: string;
  hasOuter: boolean;
  outerType: string;
  skuText: string;
  cost: number;
  inStock: number;
  refundType: RefundType;
  price: number;
  status: ProductStatus;
  freightType: FreightType;
  freightValue: number;
  freightStep: number;
  freightStepValue: number;
  num: number;
  skus: ProductSkuModel;
}

/**
 * ProductSkuModel
 */
export interface ProductSkuModel {
  tree: Array<SkuTree>;
  list: Array<SkuModel>;
  price: number;
  stock_num: number;
  collection_id: string;
  none_sku: boolean;
  hide_stock: boolean;
  specialPrices: any;
}

/**
 * SkuTree
 */
export interface SkuTree {
  k: string;
  v: Array<SkuTreeValue>;
  k_s: string;
}

/**
 * SkuTreeValue
 */
export interface SkuTreeValue {
  id: string;
  name: string;
  imgUrl: string;
}

/**
 * SkuModel
 */
export interface SkuModel {
  id: string;
  s1: string;
  key1: string;
  name1: string;
  s2: string;
  key2: string;
  name2: string;
  s3: string;
  key3: string;
  name3: string;
  cost: number;
  price: number;
  stock_num: number;
  specialPrices: any;
}

//
export enum RefundType {
  Support = 1,
  OnlyReturn = 2,
  OnlyChange = 3,
  NoSupport = 4
}
//
export enum ProductStatus {
  OnStock = 0,
  Examine = 1,
  OnSale = 2,
  Delete = -1
}
//
export enum FreightType {
  Product = 0,
  Shop = 1,
  Free = 2,
  FreightStep = 3,
  FreightArea = 4
}
