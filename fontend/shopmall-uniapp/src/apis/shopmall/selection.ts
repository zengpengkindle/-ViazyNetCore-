import { http } from "@/utils/http";

export class SelectionApi {
  /**
   * 无
   */
  public feed(
    page?: number,
    limit?: number
  ): Promise<SelectionFeedListDtoMorePageData> {
    return http.request({
      url: "/api/Selection/Feed",
      method: "post",
      params: { Page: page, Limit: limit }
    });
  }
}
export default new SelectionApi();
/**
 * SelectionFeedListDtoMorePageData
 */
export interface SelectionFeedListDtoMorePageData {
  rows: Array<SelectionFeedListDto>;
  hasMore: boolean;
}

/**
 * SelectionFeedListDto
 */
export interface SelectionFeedListDto {
  id: string;
  image: string;
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
  price: number;
}
