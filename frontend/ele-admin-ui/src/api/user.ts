import { http } from "@/utils/http";
import {
  UserFindAllArgs,
  UserFindAllModelPageData,
  UserFindModel,
  UserModel,
  UserManageDto
} from "./model";

export class UserApi {
  /**
   * 无
   */
  public apiUserFindAll(
    param1: UserFindAllArgs
  ): Promise<UserFindAllModelPageData> {
    return http.request({
      url: "/api/User/findAll",
      method: "post",
      data: param1
    });
  }
  /**
   * 无
   */
  public apiUserFind(id: string): Promise<UserFindModel> {
    return http.request({
      url: "/api/User/find",
      method: "post",
      params: { id }
    });
  }
  public apiUserManage(param1: UserModel): Promise<UserManageDto> {
    return http.request({
      url: "/api/User/manage",
      method: "post",
      data: param1
    });
  }
  public apiUserResetPassword(id?: string): Promise<string> {
    return http.request({
      url: "/api/User/resetPassword",
      method: "post",
      params: { id }
    });
  }
  /**
   * 无
   */
  public apiUserRemove(id: string): Promise<boolean> {
    return http.request({
      url: "/api/User/remove",
      method: "post",
      params: { id }
    });
  }
}
export default new UserApi();
