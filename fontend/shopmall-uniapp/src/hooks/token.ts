import { readonly, ref } from "vue";
import { TOKEN } from "@/utils/storage";

const token = ref("");
token.value = uni.getStorageSync(TOKEN) as string;

export function useToken() {
  const setToken = (t: string) => {
    token.value = t;
    uni.setStorageSync(TOKEN, t);
  };
  const clearToken = () => {
    token.value = "";
    uni.setStorageSync(TOKEN, "");
  };

  return {
    token: readonly(token),
    setToken,
    clearToken
  };
}
