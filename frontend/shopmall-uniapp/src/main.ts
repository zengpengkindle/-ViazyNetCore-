import { createSSRApp } from "vue";
import { createPinia } from "pinia";
import App from "./App.vue";
import { theme } from "@/utils/themes";

import uView from "./uni_modules/vk-uview-ui";

import Price from "./components/ui/price.vue";

export function createApp() {
  const app = createSSRApp(App);
  app.use(createPinia());
  app.use(uView);
  app.component("Price", Price);
  app.config.globalProperties.$themes = theme;
  return {
    app
  };
}
