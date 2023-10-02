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
export interface ApiItemDto {
  path: string;
  httpMethod: string;
}

/**
 * PermissionApiUpdateDto
 */
export interface PermissionApiUpdateDto {
  permissionKey: string | null;
  apis: Array<ApiItemDto> | null;
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

  public apiPermissionGetMenuKeysInPermissionKey(
    permissionKey: string
  ): Promise<Array<string>> {
    return http.request({
      url: "/api/Permission/getMenuKeysInPermissionKey",
      method: "post",
      params: { permissionKey }
    });
  }
  /**
   * 无
   */
  public apiPermissionUpdateMenusInPermission(
    permissionKey?: string,
    param1?: Array<string>
  ): Promise<boolean> {
    return http.request({
      url: "/api/Permission/updateMenusInPermission",
      method: "post",
      data: param1,
      params: { permissionKey }
    });
  }
  public apiPermissionUpdatePermissionsInRole(
    roleId?: string,
    param1?: Array<string>
  ): Promise<boolean> {
    return http.request({
      url: "/api/Permission/updatePermissionsInRole",
      method: "post",
      data: param1,
      params: { roleId }
    });
  }
  /**
   * 无
   */
  public getApisInPermissionKey(
    permissionKey?: string
  ): Promise<Array<ApiItemDto>> {
    return http.request({
      url: "/api/Permission/getApisInPermissionKey",
      method: "post",
      params: { permissionKey }
    });
  }

  public updateApisInPermission(
    param1?: PermissionApiUpdateDto
  ): Promise<boolean> {
    return http.request({
      url: "/api/Permission/updateApisInPermission",
      method: "post",
      data: param1
    });
  }
}
export default new PermissionApi();
