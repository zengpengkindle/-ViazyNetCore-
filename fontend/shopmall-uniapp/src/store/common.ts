import { ref } from "vue";
import { defineStore } from "pinia";
import { useToken } from "@/hooks";
import WechatApi from "@/apis/wechat";

export type EnvVersion = "develop" | "trial" | "release";

export const useCommonStore = defineStore("common", () => {
  const { token } = useToken();
  /**
   * 获取当前运行环境 开发版 体验版 正式版
   */
  const env = ref<EnvVersion>();
  const getEnvVersion = () => {
    env.value = uni.getAccountInfoSync().miniProgram.envVersion;
  };
  /**
   * 微信唯一标识
   */
  const uid = ref("");
  const getUid = () => {
    return new Promise<void>(resolve => {
      uni.login({
        provider: "weixin",
        async success(result) {
          const response = await WechatApi.secUnionid({ code: result.code });
          uid.value = response.secUnionId;
          resolve();
        }
      });
    });
  };

  /**
   * 检查更新
   */
  const checkUpdate = () => {
    const manager = uni.getUpdateManager();
    manager.onUpdateReady(() => {
      uni.showModal({
        title: "发现新版本",
        content: "检测到小程序有新版本，请更新后使用",
        confirmText: "更新",
        success(res) {
          if (res.confirm) {
            manager.applyUpdate();
          }
        }
      });
    });
    manager.onUpdateFailed(res => {
      console.log("更新版本下载失败", res);
      const log = uni.getRealtimeLogManager();
      log.error([res]);
    });
  };
  return {
    env,
    token,
    getEnvVersion,
    checkUpdate,
    getUid
  };
});
