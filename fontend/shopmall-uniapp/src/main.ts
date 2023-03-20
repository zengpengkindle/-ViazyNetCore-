import { createSSRApp } from "vue";
import { createPinia } from "pinia";
import App from "./App.vue";

import uView from "./uni_modules/vk-uview-ui";
export function createApp() {
  const app = createSSRApp(App);
  app.use(createPinia());
  app.use(uView);
  return {
    app
  };
}
