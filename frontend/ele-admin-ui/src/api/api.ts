import { http } from "@/utils/http";
/**
 * ApiGroupDescriptor
 */
export interface ApiGroupDescriptor {
  controllerName: string;
  displayControllerName: string;
  apis: Array<ApiDescriptor>;
}

/**
 * ApiDescriptor
 */
export interface ApiDescriptor {
  id: string;
  serviceName: string;
  apiVersion: string;
  controllerName: string;
  displayControllerName: string;
  actionName: string;
  displayActionName: string;
  actionDescriptor: string;
  routeTemplate: string;
  routePath: string;
  httpMethod: string;
}

export class Api {
  /**
   * 无
   */
  public getApis(): Promise<Array<ApiGroupDescriptor>> {
    return http.request({
      url: "/api/Api/getApis",
      method: "post"
    });
  }
}
export default new Api();
