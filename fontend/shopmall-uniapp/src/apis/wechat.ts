/**
 * JsCodeReq
 */

import { http } from "@/utils/http";
export interface JsCodeReq {
  code: string | null;
}

/**
 * UnionIdRes
 */
export interface UnionIdRes {
  secUnionId: string;
}

export class WeChatApi {
  /**
   * 无
   */
  public secUnionid(param1?: JsCodeReq): Promise<UnionIdRes> {
    return http.request({
      url: "/api/wechat/wx/sec_unionid",
      method: "post",
      data: param1
    });
  }
}
export default new WeChatApi();
