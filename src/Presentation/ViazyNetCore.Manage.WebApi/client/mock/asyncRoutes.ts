// 模拟后端动态生成路由
import { MockMethod } from "vite-plugin-mock";
import { ApiResponseMockDefault } from "./mockResBase";

/**
 * roles：页面级别权限，这里模拟二种 "admin"、"common"
 * admin：管理员角色
 * common：普通角色
 */

const permissionRouter = {
  path: "/permission",
  meta: {
    title: "权限管理",
    icon: "lollipop",
    rank: 10
  },
  children: [
    {
      path: "/permission/page/index",
      name: "PermissionPage",
      meta: {
        title: "页面权限",
        roles: ["admin", "common"]
      }
    },
    {
      path: "/permission/button/index",
      name: "PermissionButton",
      meta: {
        title: "按钮权限",
        roles: ["admin", "common"],
        auths: ["btn_add", "btn_edit", "btn_delete"]
      }
    }
  ]
};

const systemRouter = {
  path: "/system",
  meta: {
    icon: "setting",
    title: "系统管理",
    rank: 11
  },
  children: [
    {
      path: "/system/user/index",
      name: "User",
      meta: {
        icon: "flUser",
        title: "用户管理",
        roles: ["admin"]
      }
    },
    {
      path: "/system/role/index",
      name: "Role",
      meta: {
        icon: "role",
        title: "角色管理",
        roles: ["admin"]
      }
    },
    {
      path: "/system/menu/index",
      name: "Menu",
      meta: {
        icon: "menu",
        title: "菜单管理",
        roles: ["admin"]
      }
    },
    {
      path: "/system/permission/index",
      name: "Permission",
      meta: {
        icon: "lollipop",
        title: "权限管理",
        roles: ["admin"]
      }
    }
    // {
    //   path: "/system/dept/index",
    //   name: "Dept",
    //   meta: {
    //     icon: "dept",
    //     title: "部门管理",
    //     roles: ["admin"]
    //   }
    // }
  ]
};

export default [
  {
    url: "/getAsyncRoutes",
    method: "get",
    response: () => {
      return new ApiResponseMockDefault([permissionRouter, systemRouter]);
    }
  }
] as MockMethod[];
