/**
 * JsCodeReq
 */

import { http } from "@/utils/http";

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
  /**
   * 无
   */
  public getJscode(param1?: JsCodeReq): Promise<JsCodeRes> {
    return http.request({
      url: "/api/wechat/wx/getJscode",
      method: "post",
      data: param1
    });
  }
  /**
   * 无
   */
  public bindmobile(param1?: WechatBindMobileReq): Promise<JsCodeRes> {
    return http.request({
      url: "/api/wechat/wx/bindmobile",
      method: "post",
      data: param1
    });
  }
}
export default new WeChatApi();

export interface JsCodeReq {
  code: string | null;
}
/**
 * JsCodeRes
 */
export interface JsCodeRes {
get_user_profile: any;
  openId: string;
  getUserProfile: boolean;
  token: JwtTokenResult;
  opType: number;
  isBindMobile: boolean;
  authCode: string;
}
/**
 * JwtTokenResult
 */
export interface JwtTokenResult {
access_token(access_token: any): unknown;
  /** 获取到的凭证 */ accessToken: string;
  /** 凭证有效时间，单位：秒 */ expiresIn: number;
}
/**
 * UnionIdRes
 */
export interface UnionIdRes {
  secUnionId: string;
}
/**
 * WechatBindMobileReq
 */
export interface WechatBindMobileReq {
  authCode: string | null;
  mobile: string | null;
  smsCode: string | null;
  encryptedData: string | null;
  iv: string | null;
}
