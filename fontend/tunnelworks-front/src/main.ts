import App from "./App.vue";
import router from "./router";
import { setupStore } from "@/store";
import ElementPlus from "element-plus";
import { getServerConfig } from "./config";
import { createApp, Directive } from "vue";
import { MotionPlugin } from "@vueuse/motion";
import createGlobal from "@/utils/globals";
// import { useEcharts } from "@/plugins/echarts";
import { injectResponsiveStorage } from "@/utils/responsive";
import { registerIcon } from "@/utils/el-icons";
import "virtual:svg-icons-register";

import Table from "@pureadmin/table";
import Status from "@/components/ui/status.vue";
import XTable from "@/components/Table";
import ImageUpload from "@/components/ui/image.vue";
import CofirmButton from "@/components/ui/cofirm-button.vue";

// import * as Draggable from "@/../lib/vuedraggable/dist/vuedraggable.umd.js";
import * as Draggable from "vuedraggable";
// import PureDescriptions from "@pureadmin/descriptions";

// 引入重置样式
import "./style/reset.scss";
// 导入公共样式
import "./style/index.scss";
// 一定要在main.ts中导入tailwind.css，防止vite每次hmr都会请求src/style/index.scss整体css文件导致热更新慢的问题
import "./style/tailwind.css";
import "element-plus/dist/index.css";
// 导入字体图标
import "./assets/iconfont/iconfont.js";
import "./assets/iconfont/iconfont.css";

const app = createApp(App);
registerIcon(app);

// 自定义指令
import * as directives from "@/directives";
Object.keys(directives).forEach(key => {
  app.directive(key, (directives as { [key: string]: Directive })[key]);
});

// 全局注册`@iconify/vue`图标库
import {
  IconifyIconOffline,
  IconifyIconOnline,
  FontIcon
} from "./components/ReIcon";
app.component("IconifyIconOffline", IconifyIconOffline);
app.component("IconifyIconOnline", IconifyIconOnline);
app.component("FontIcon", FontIcon);

// 全局注册按钮级别权限组件
import { Auth } from "@/components/ReAuth";
app.component("Auth", Auth);
app.component("x-status", Status);
app.component("x-image", ImageUpload);
app.component("x-cofirm-button", CofirmButton);
app.component("draggable", Draggable);

getServerConfig(app).then(async config => {
  app.use(router);
  await router.isReady();
  injectResponsiveStorage(app, config);
  setupStore(app);
  app.use(MotionPlugin).use(ElementPlus);
  app
    .use(createGlobal())
    // .use(useEcharts)
    .use(Table)
    .use(XTable); // .use(PureDescriptions)
  app.mount("#app");
});
