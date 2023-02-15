import { http } from "@/utils/http";
import { ComStatus } from "./model";
/**
 * PermissionKey
 */
export interface PermissionKey {
  key: string;

  id: number;

  name: string;
}

/**
 * MenuTreeModel
 */
export interface MenuTreeModel {
  id: string;

  label: string;

  type: MenuType;

  icon: string;

  children: Array<MenuTreeModel>;
}
export enum MenuType {
  Node = 0,
  MidNode = 1,
  Button = 2
}

/**
 * BmsMenus
 */
export interface BmsMenus {
  parentId: string;

  name: string;

  type: MenuType;

  url: string;

  sysId: string;

  orderId: number;

  status: ComStatus;

  description: string;

  createTime: string;

  projectId: string;

  openType: number;

  isHomeShow: boolean;

  icon: string;

  exdata: string;

  id: string;
}

export class PermissionApi {
  /**
   * 无
   */
  public apiPermissionGetAll(): Promise<Array<PermissionKey>> {
    return http.request({
      url: "/api/Permission/getAll",
      method: "get"
    });
  }
  /**
   * 无
   */
  public apiPermissionGetRolePermission(
    roleId?: string
  ): Promise<Array<string>> {
    return http.request({
      url: "/api/Permission/getRolePermission",
      method: "get",
      params: { roleId }
    });
  }
  public apiPermissionGetMenusTree(): Promise<Array<MenuTreeModel>> {
    return http.request({
      url: "/api/Permission/getMenusTree",
      method: "post"
    });
  }
  /**
   * 无
   */
  public apiPermissionGetMenus(): Promise<Array<BmsMenus>> {
    return http.request({
      url: "/api/Permission/getMenus",
      method: "post"
    });
  }
  /**
   * 无
   */
  public apiPermissionGetMenu(id?: string): Promise<BmsMenus> {
    return http.request({
      url: "/api/Permission/getMenu",
      method: "post",
      params: { id }
    });
  }
  /**
   * 无
   */
  public apiPermissionUpdateMenu(param1?: BmsMenus): Promise<boolean> {
    return http.request({
      url: "/api/Permission/updateMenu",
      method: "post",
      data: param1
    });
  }
  /**
   * 无
   */
  public apiPermissionRemoveMenu(menuId?: string): Promise<boolean> {
    return http.request({
      url: "/api/Permission/removeMenu",
      method: "post",
      params: { menuId }
    });
  }
}
export default new PermissionApi();
