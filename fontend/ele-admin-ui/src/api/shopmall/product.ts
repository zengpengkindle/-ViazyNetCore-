import { http } from "@/utils/http";
import { SingleOrRange } from "element-plus";
import { ComStatus } from "../model";
/**
 * FindAllArguments
 */
export interface FindAllArguments {
  shopId: string | null;
  titleLike: string | null;
  catName: string | null;
  isHidden: boolean | null;
  status: ProductStatus | null;
  createTimes: SingleOrRange<string> | null;
  page: number | null;
  limit: number | null;
}
/**
 * ProductPageData
 */
export interface ProductPageData {
  rows: Array<Product>;
  total: number;
}

/**
 * Product
 */
export interface Product {
  brandId: string;
  brandName: string;
  catId: string;
  catName: string;
  shopId: string;
  shopName: string;
  catPath: string;
  title: string;
  subTitle: string;
  keywords: string;
  description: string;
  cost: number;
  price: number;
  isFreeFreight: boolean;
  freight: number;
  freightStep: number;
  freightValue: number;
  isHidden: boolean;
  status: ProductStatus;
  statusChangeTime: string;
  image: string;
  subImage: string;
  openSpec: boolean;
  skuTree: string;
  detail: string;
  createTime: string;
  modifyTime: string;
  searchContent: string;
  hasOuter: boolean;
  outerType: string;
  refundType: RefundType;
  exdata: string;
  id: string;
}
/**
 * ProductManageModel
 */
export interface ProductManageModel {
  id: string | null;
  brandId: string | null;
  brandName: string | null;
  catId: string | null;
  catName: string | null;
  shopId: string | null;
  shopName: string | null;
  catPath: string | null;
  title: string | null;
  subTitle: string | null;
  keywords: string | null;
  description: string | null;
  cost: number | null;
  price: number | null;
  isFreeFreight: boolean | null;
  freight: number | null;
  freightStep: number | null;
  freightValue: number | null;
  isHidden: boolean | null;
  statusChangeTime: string | null;
  image: string | null;
  subImage: string | null;
  openSpec: boolean | null;
  detail: string | null;
  status: ProductStatus | null;
  refundType: RefundType | null;
  stock: StockModel | null;
  createTime: string | null;
  modifyTime: string | null;
  searchContent: string | null;
  hasOuter: boolean | null;
  outerType: string | null;
  exdata: string | null;
  propertyKeys: string | null;
  propertyValues: string | null;
  specialPrices: any | null;
  skus: ProductSkuManageModel | null;
}

/**
 * StockModel
 */
export interface StockModel {
  id: string;
  productId: string;
  title: string;
  imgUrl: string;
  inStock: number;
  lock: number;
  outStock: number;
  refund: number;
  sellNum: number;
  exchange: number;
  skus: Array<StockSkuModel>;
}
export interface ProductModifyModel {
  product: ProductManageModel;
  brands: Array<ProductBrand>;
  cats: Array<ProductCat>;
}
/**
 * StockSkuModel
 */
export interface StockSkuModel {
  id: string;
  productSkuId: string;
  skuText: string;
  inStock: number;
  lock: number;
  outStock: number;
  sellNum: number;
  refund: number;
  exchange: number;
}
/**
 * ProductBrand
 */
export interface ProductBrand {
  isHidden: boolean;
  name: string;
  image: string;
  status: ComStatus;
  sort: number;
  exdata: string;
  id: string;
}

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
/**
 * ProductSkuManageModel
 */
export interface ProductSkuManageModel {
  tree: Array<SkuTree>;
  list: Array<SkuModel>;
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
export enum ProductStatus {
  OnStock = 0,
  Examine = 1,
  OnSale = 2,
  Delete = -1
}
//
export enum RefundType {
  Support = 1,
  OnlyReturn = 2,
  OnlyChange = 3,
  NoSupport = 4
}

export class ProductApi {
  /**
   * 无
   */
  public submit(
    outerType?: string,
    param1?: ProductManageModel
  ): Promise<void> {
    return http.request({
      url: "/api/Product/Submit",
      method: "post",
      data: param1,
      params: { outerType }
    });
  }
  /**
   * 无
   */
  public find(id?: string, outerType?: string): Promise<ProductModifyModel> {
    return http.request({
      url: "/api/Product/Find",
      method: "post",
      params: { id, outerType }
    });
  }
  public findAll(param1?: FindAllArguments): Promise<ProductPageData> {
    return http.request({
      url: "/api/Product/FindAll",
      method: "post",
      data: param1
    });
  }
  /**
   * 无
   */
  public modifyStatus(id?: string, status?: ProductStatus): Promise<boolean> {
    return http.request({
      url: "/api/Product/ModifyStatus",
      method: "post",
      params: { id, status }
    });
  }
}
export default new ProductApi();
