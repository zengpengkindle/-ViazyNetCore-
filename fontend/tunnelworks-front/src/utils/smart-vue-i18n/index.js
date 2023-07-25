import { reactive } from "vue";
import { get } from "./utils";

const locale = reactive({
  lang: localStorage.getItem("v_form_locale") || "zh-CN"
});

export function createI18n(options) {
  return {
    messages: options.messages,
    $st(path, ...args) {
      const message = get(this.messages[locale.lang], path);
      return typeof message === "function"
        ? message(...args)
        : message !== null
        ? message
        : path;
    },

    $st2(path, path2) {
      const messages = this.messages[locale.lang];
      const message = get(messages, path);
      return message !== null ? message : get(messages, path2);
    },

    setLang(lang) {
      locale.lang = lang;
    }
  };
}
