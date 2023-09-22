const Layout = () => import("@/layout/index.vue");

export default [
  {
    path: "/login",
    name: "Login",
    component: () => import("@/views/login/index.vue"),
    meta: {
      title: "登录",
      showLink: false,
      rank: 101
    }
  },
  {
    path: "/redirect",
    component: Layout,
    meta: {
      icon: "homeFilled",
      title: "首页",
      showLink: false,
      rank: 104
    },
    children: [
      {
        path: "/redirect/:path(.*)",
        name: "Redirect",
        component: () => import("@/layout/redirect.vue")
      }
    ]
  },
  {
    path: "/account",
    component: Layout,
    meta: {
      title: "账号管理",
      showLink: false,
      rank: 104
    },
    children: [
      {
        path: "/account/settings",
        name: "accountSettings",
        component: () => import("@/views/account/settings.vue"),
        meta: {
          title: "个人信息",
          showLink: false
        }
      }
    ]
  },
  {
    path: "/shopmall",
    component: Layout,
    meta: {
      title: "商品管理",
      showLink: false,
      rank: 104
    },
    children: [
      {
        path: "/shopmall/product/manage",
        name: "productManage",
        component: () => import("@/views/shopmall/product/manage.vue"),
        meta: {
          title: "商品编辑"
        }
      },
      {
        path: "/shopmall/productOuterSpecialCredit/manage",
        name: "productOuterSpecialCreditManage",
        component: () =>
          import("@/views/shopmall/productOuterSpecialCredit/manage.vue"),
        meta: {
          title: "类别/活动管理"
        }
      },
      {
        path: "/shopmall/Trade/Manage",
        name: "productOuterSpecialCreditManage",
        component: () => import("@/views/shopmall/Trade/Manage.vue"),
        meta: {
          title: "订单管理"
        }
      }
    ]
  },
  {
    path: "/shopmall/page/manage",
    name: "shopPageManage",
    component: () => import("@/views/shopmall/page/manage.vue"),
    meta: {
      title: "页面设计",
      showLink: false
    }
  }
] as Array<RouteConfigsTable>;
