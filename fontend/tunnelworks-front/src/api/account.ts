import { http } from "@/utils/http";
import { UserLoginArgs, JwtTokenResult } from "./model";
export interface UserModifyPasswordArgs {
  oldPassword: string | null;
  newPassword: string | null;
}

export interface UserIdentity {
  id: number | null;
  clientId: string;
  nbf: number;
  exp: number;
  amr: string;
  username: string;
  nickname: string;
  tenantId: number;
}

export class AccountApi {
  /**
   * 无
   */
  public accountLogin(param1: UserLoginArgs): Promise<JwtTokenResult> {
    return http.request({
      url: "/api/account/login",
      method: "post",
      data: param1
    });
  }

  public getIdentity(): Promise<UserIdentity> {
    return http.request({
      url: "/api/account/getIdentity",
      method: "post"
    });
  }

  public modifyPassword(param1: UserModifyPasswordArgs): Promise<boolean> {
    return http.request({
      url: "/api/Account/modifyPassword",
      method: "post",
      data: param1
    });
  }
}
export default new AccountApi();
