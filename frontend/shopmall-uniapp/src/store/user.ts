import { ref } from "vue";
import { defineStore } from "pinia";
import { useToken } from "@/hooks";
import type { JsCodeRes } from "@/apis/wechat";
export type UserAssets = {
  balance: number;
};

export const useUserStore = defineStore("user", () => {
  const { token } = useToken();

  // 用户信息
  const info = ref();
  const getInfo = async (forceUpdate = true) => {
    if (!forceUpdate && Date.now() - 0 < 5 * 60 * 1000) {
      return;
    }
  };
  const clearInfo = () => {
    info.value = null;
  };

  const setLoginInfo = (res: JsCodeRes) => {
    loginInfo.value = res;
  };

  /**
   * 登录信息
   */
  const loginInfo = ref<JsCodeRes>();
  return {
    info,
    getInfo,
    clearInfo,
    token,
    loginInfo,
    setLoginInfo
  };
});
