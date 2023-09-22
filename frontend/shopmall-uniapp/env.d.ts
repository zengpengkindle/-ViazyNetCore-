/// <reference types="vite/client" />
/// <reference types="@dcloudio/types" />

declare interface ImportMetaEnv {
  /**
   * 正式站接口地址
   */
  VITE_FORMAL_BASE_URL: string;
  /**
   * 本地开发/开发版/体验版 接口地址
   */
  VITE_BASE_URL: string;
}
