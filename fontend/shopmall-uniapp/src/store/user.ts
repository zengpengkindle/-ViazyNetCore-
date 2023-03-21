import { ref } from "vue";
import { defineStore } from "pinia";
import { useToken } from "@/hooks";

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

  return {
    info,
    getInfo,
    clearInfo
  };
});
