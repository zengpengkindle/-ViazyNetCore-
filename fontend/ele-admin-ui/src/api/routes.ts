import { http } from "@/utils/http";

type Result = {
  data: Array<any>;
};

export const getAsyncRoutes = () => {
  return http.request<Result>({ method: "get", url: "/getAsyncRoutes" });
};
/**
 * PermissionRouterModel
 */
export interface PermissionRouterModel {
  path: string;

  name: string;

  meta: PermissionRouteMeta;

  children: Array<PermissionRouterModel>;
}

/**
 * PermissionRouteMeta
 */
export interface PermissionRouteMeta {
  title: string;

  icon: string;

  rank: number;

  roles: Array<string>;
}

export class RouterApi {
  /**
   * 无
   */
  public apiPermissionGetUserRouters(): Promise<Array<PermissionRouterModel>> {
    return http.request({
      url: "/api/Permission/getUserRouters",
      method: "post"
    });
  }
}
export default new RouterApi();
