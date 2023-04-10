import { ref } from "vue";
import { defineStore } from "pinia";
import { useToken } from "@/hooks";

export type UserAssets = {
  balance: number;
};
export type WxLoginResponse = {
  /**
   * 临时令牌
   */
  auth_code: string;
  /**
   * 是否需要唤起 wx.getUserProfile, 获取昵称和头像
   */
  get_user_profile: boolean;
  /**
   * 是否已绑定手机
   */
  is_bind_mobile: boolean;
  /**
   * 1:注册 2:登录
   */
  op_type: number;
  /**
   * 小程序openid
   */
  open_id: string;
  token: {
    /**
     * 获取到的凭证
     */
    access_token: string;
    /**
     * 凭证有效时间，单位：秒
     */
    expires_in: number;
  };
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

  /**
   * 登录信息
   */
  const loginInfo = ref<WxLoginResponse>();
  return {
    info,
    getInfo,
    clearInfo,
    token,
    loginInfo
  };
});
