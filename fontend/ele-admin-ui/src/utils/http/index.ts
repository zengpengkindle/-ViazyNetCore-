import Axios, {
  AxiosInstance,
  AxiosRequestConfig,
  CustomParamsSerializer
} from "axios";
import {
  ViazyHttpError,
  ViazyHttpResponse,
  ViazyHttpRequestConfig
} from "./types.d";
import { stringify } from "qs";
import NProgress from "../progress";
import { getToken, formatToken, removeToken } from "@/utils/auth";
import { message } from "@/utils/message";
import { ApiResponse, ApiReponseError } from "@/api/model/apiResponseBase";
import route from "@/router";

// 相关配置请参考：www.axios-js.com/zh-cn/docs/#axios-request-config-1
const defaultConfig: AxiosRequestConfig = {
  // 请求超时时间
  timeout: 10000,
  headers: {
    Accept: "application/json, text/plain, */*",
    "Content-Type": "application/json",
    "X-Requested-With": "XMLHttpRequest"
  },
  // 数组格式参数序列化（https://github.com/axios/axios/issues/5142）
  paramsSerializer: {
    serialize: stringify as unknown as CustomParamsSerializer
  }
};

const whiteList = ["/refreshToken", "/login", "/acount/login"];

class ViazyHttp {
  constructor() {
    this.httpInterceptorsRequest();
    this.httpInterceptorsResponse();
  }

  /** token过期后，暂存待执行的请求 */
  private static requests = [];

  /** 防止重复刷新token */
  private static isRefreshing = false;

  /** 初始化配置对象 */
  private static initConfig: ViazyHttpRequestConfig = {
    headers: null,
    beforeResponseCallback: (response: ViazyHttpResponse) => {
      const apiResponse = response.data as ApiResponse;
      if (apiResponse.code == 200) {
        if (apiResponse.data.success) {
          return apiResponse.data.result;
        } else {
          throw new ApiReponseError(
            apiResponse.code,
            apiResponse.data.err_code,
            apiResponse.message,
            apiResponse.data.message
          );
          // throw new AxiosError(
          //   apiResponse.message,
          //   "200",
          //   null,
          //   null,
          //   response
          // );
        }
      }
    }
  };

  /** 保存当前Axios实例对象 */
  private static axiosInstance: AxiosInstance = Axios.create(defaultConfig);

  /** 重连原始请求 */
  private static retryOriginalRequest(
    config: ViazyHttpRequestConfig
  ): Promise<ViazyHttpRequestConfig<any>> {
    return new Promise(resolve => {
      ViazyHttp.requests.push((token: string) => {
        config.headers["Authorization"] = formatToken(token);
        resolve(config);
      });
    });
  }

  /** 请求拦截 */
  private httpInterceptorsRequest(): void {
    ViazyHttp.axiosInstance.interceptors.request.use(
      async (config: ViazyHttpRequestConfig) => {
        // 开启进度条动画
        NProgress.start();
        // 优先判断post/get等方法是否传入回掉，否则执行初始化设置等回掉
        if (typeof config.beforeRequestCallback === "function") {
          config.beforeRequestCallback(config);
          return config;
        }
        if (ViazyHttp.initConfig.beforeRequestCallback) {
          ViazyHttp.initConfig.beforeRequestCallback(config);
          return config;
        }
        /** 请求白名单，放置一些不需要token的接口（通过设置请求白名单，防止token过期后再请求造成的死循环问题） */
        return whiteList.some(v => config.url.indexOf(v) > -1)
          ? config
          : new Promise(resolve => {
              const data = getToken();
              if (data) {
                // const now = new Date().getTime();
                // const expired = parseInt(data.expires) - now <= 0;
                // if (expired) {
                //   if (!PureHttp.isRefreshing) {
                //     PureHttp.isRefreshing = true;
                //     // token过期刷新
                //     useUserStoreHook()
                //       .handRefreshToken({ refreshToken: data.refreshToken })
                //       .then(res => {
                //         const token = res.data.accessToken;
                //         config.headers["Authorization"] = formatToken(token);
                //         PureHttp.requests.forEach(cb => cb(token));
                //         PureHttp.requests = [];
                //       })
                //       .finally(() => {
                //         PureHttp.isRefreshing = false;
                //       });
                //   }
                //   resolve(PureHttp.retryOriginalRequest(config));
                // } else {
                config.headers["Authorization"] = formatToken(data.accessToken);
                resolve(config);
                // }
              } else {
                resolve(config);
              }
            });
      },
      error => {
        return Promise.reject(error);
      }
    );
  }
  openErrorMessage = (msg: string): void => {
    const isMessageLen = document.getElementsByClassName(
      "el-message el-message--error"
    ).length;
    if (isMessageLen !== 0) {
      return;
    } else {
      message(msg, { type: "error" });
    }
  };
  /** 响应拦截 */
  private httpInterceptorsResponse(): void {
    const instance = ViazyHttp.axiosInstance;
    instance.interceptors.response.use(
      (response: ViazyHttpResponse) => {
        const $config = response.config;
        // 关闭进度条动画
        NProgress.done();
        console.log(`api resquest success`);
        // 优先判断post/get等方法是否传入回掉，否则执行初始化设置等回掉
        if (typeof $config.beforeResponseCallback === "function") {
          return $config.beforeResponseCallback(response);
          // return response.data;
        }
        if (ViazyHttp.initConfig.beforeResponseCallback) {
          return ViazyHttp.initConfig.beforeResponseCallback(response);
          // response.data;
        }
        return response.data;
      },
      (error: ViazyHttpError) => {
        const $error = error;
        $error.isCancelRequest = Axios.isCancel($error);
        // 关闭进度条动画
        NProgress.done();
        if (!error.response) {
          return Promise.reject($error);
        } else {
          const response = error.response.data as ApiResponse;
          // console.log(`error message:`, response.data?.message);
          if (response.code === 0) {
            return Promise.reject($error);
          } else if (response.code === 200) {
            message(response.data?.message, {
              type: "error",
              customClass: "antd"
            });
          } else if (response.code === 400) {
            message(response.message, { type: "error" });
          } else if (response.code === 401) {
            // router.push("/login");
            removeToken();
            this.openErrorMessage(`登录过期,请重新登录`);
            route.push({ path: "/login" });
          } else if (response.code === 403) {
            message("您的权限不足！", {
              type: "error",
              customClass: "antd"
            });
          } else if (response.code === 406) {
            message(response.message, {
              type: "error",
              customClass: "antd"
            });
          } else if (response.code === 409) {
            message(`您的账户已在其他设备登录！`, {
              type: "warning",
              customClass: "antd"
            });
            removeToken();
            route.push({ name: "/login" });
          } else if (response.code === 500) {
            message(`Oop~ 服务器繁忙,请稍候再试！` + response.message, {
              type: "error",
              customClass: "el"
            });
          }
        }
        // 所有的响应异常 区分来源为取消请求/非取消请求
        return Promise.reject($error);
      }
    );
  }

  /** 通用请求工具函数 */
  public request<T>(
    param: AxiosRequestConfig,
    axiosConfig?: ViazyHttpRequestConfig
  ): Promise<T> {
    const config = {
      ...param,
      ...axiosConfig
    } as ViazyHttpRequestConfig;
    const baseURL = param.baseURL || import.meta.env.VITE_APP_BASE_URL;
    config.baseURL = baseURL;
    // 单独处理自定义请求/响应回掉
    return new Promise((resolve, reject) => {
      ViazyHttp.axiosInstance
        .request(config)
        .then((response: undefined) => {
          resolve(response);
        })
        .catch(error => {
          reject(error);
        });
    });
  }

  /** 单独抽离的post工具函数 */
  public post<T, P>(
    url: string,
    params?: AxiosRequestConfig<T>,
    config?: ViazyHttpRequestConfig
  ): Promise<P> {
    return this.request<P>({ method: "post", url, ...params }, config);
  }

  /** 单独抽离的get工具函数 */
  public get<T, P>(
    url: string,
    params?: AxiosRequestConfig<T>,
    config?: ViazyHttpRequestConfig
  ): Promise<P> {
    return this.request<P>({ method: "get", url, ...params }, config);
  }
}

export const http = new ViazyHttp();
