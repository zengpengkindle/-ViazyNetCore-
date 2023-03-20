import { ref } from "vue";
import { defineStore } from "pinia";
import { useToken } from "@/hooks";
import { userInfo, type UserInfo } from "@/apis/user";
import { wxCode, type WxLoginResponse } from "@/apis/login";
import { asyncLogin } from "@/utils";

export type UserAssets = {
  balance: number;
};

export const useUserStore = defineStore("user", () => {
  const { token } = useToken();

  // 用户信息
  const info = ref<UserInfo>();
  let userinfoFetchTime = 0;
  const getInfo = async (forceUpdate = true) => {
    if (!forceUpdate && Date.now() - userinfoFetchTime < 5 * 60 * 1000) {
      return;
    }
    if (token.value) {
      const res = (await userInfo()) as UserInfo;
      userinfoFetchTime = Date.now();
      info.value = res;
    } else {
      info.value = undefined;
    }
  };
  const clearInfo = () => {
    info.value = undefined;
  };


  /**
   * 登录信息
   */
  const loginInfo = ref<WxLoginResponse>();
  const login = async () => {
    const loginRes = await asyncLogin();
    loginInfo.value = await wxCode({ code: loginRes.code });
  };
  const setLoginInfo = (res: WxLoginResponse) => {
    loginInfo.value = res;
  };

  return {
    info,
    getInfo,
    clearInfo,

    loginInfo,
    login,
    setLoginInfo,
  };
});
