import { useCommonStore } from "@/store/common";

export function loginProcess() {
  const common = useCommonStore();

  // 重定向页面跳转
  if (common.redirectPath) {
    uni.reLaunch({
      url: common.redirectPath
    });

    // 清空重定向地址
    common.clearRedirectPath();
    return;
  }

  uni.reLaunch({
    url: "/pages/index/custom/index"
  });
}
