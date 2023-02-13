import Axios, {
  AxiosInstance,
  AxiosRequestConfig,
  CustomParamsSerializer,
} from "axios";
import {
  PureHttpError,
  PureHttpResponse,
  PureHttpRequestConfig
} from "./types.d";
import { stringify } from "qs";
import NProgress from "../progress";
import { getToken, formatToken } from "@/utils/auth";
import { message } from "@/utils/message";
import { ApiResponse, ApiReponseError } from "@/api/model/apiResponseBase";

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

class PureHttp {
  constructor() {
    this.httpInterceptorsRequest();
    this.httpInterceptorsResponse();
  }

  /** token过期后，暂存待执行的请求 */
  private static requests = [];

  /** 防止重复刷新token */
  private static isRefreshing = false;

  /** 初始化配置对象 */
  private static initConfig: PureHttpRequestConfig = {
    headers: null,
    beforeResponseCallback: (response: PureHttpResponse) => {
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
    config: PureHttpRequestConfig
  ): Promise<PureHttpRequestConfig<any>> {
    return new Promise(resolve => {
      PureHttp.requests.push((token: string) => {
        config.headers["Authorization"] = formatToken(token);
        resolve(config);
      });
    });
  }

  /** 请求拦截 */
  private httpInterceptorsRequest(): void {
    PureHttp.axiosInstance.interceptors.request.use(
      async (config: PureHttpRequestConfig) => {
        // 开启进度条动画
        NProgress.start();
        // 优先判断post/get等方法是否传入回掉，否则执行初始化设置等回掉
        if (typeof config.beforeRequestCallback === "function") {
          config.beforeRequestCallback(config);
          return config;
        }
        if (PureHttp.initConfig.beforeRequestCallback) {
          PureHttp.initConfig.beforeRequestCallback(config);
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

  /** 响应拦截 */
  private httpInterceptorsResponse(): void {
    const instance = PureHttp.axiosInstance;
    instance.interceptors.response.use(
      (response: PureHttpResponse) => {
        const $config = response.config;
        // 关闭进度条动画
        NProgress.done();
        console.log(`api resquest success`);
        // 优先判断post/get等方法是否传入回掉，否则执行初始化设置等回掉
        if (typeof $config.beforeResponseCallback === "function") {
          return $config.beforeResponseCallback(response);
          // return response.data;
        }
        if (PureHttp.initConfig.beforeResponseCallback) {
          return PureHttp.initConfig.beforeResponseCallback(response);
          // response.data;
        }
        return response.data;
      },
      (error: PureHttpError) => {
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
            message(response.message || `登录过期`, { type: "error" });
          } else if (response.code === 403) {
            msg.error(`您的权限不足！`);
          } else if (response.code === 406) {
            msg.error(response.message || ``);
          } else if (response.code === 409) {
            msg.warning(`您的账号已在其他地方登录！`);
          } else if (response.code === 500) {
            msg.error(`Oop~ 服务器繁忙,请稍候再试`);
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
    axiosConfig?: PureHttpRequestConfig
  ): Promise<T> {
    const config = {
      ...param,
      ...axiosConfig
    } as PureHttpRequestConfig;
    const baseURL = param.baseURL || import.meta.env.VITE_APP_BASE_URL;
    config.baseURL = baseURL;
    // 单独处理自定义请求/响应回掉
    return new Promise((resolve, reject) => {
      PureHttp.axiosInstance
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
    config?: PureHttpRequestConfig
  ): Promise<P> {
    return this.request<P>({ method: "post", url, ...params }, config);
  }

  /** 单独抽离的get工具函数 */
  public get<T, P>(
    url: string,
    params?: AxiosRequestConfig<T>,
    config?: PureHttpRequestConfig
  ): Promise<P> {
    return this.request<P>({ method: "get", url, ...params }, config);
  }
}

export const http = new PureHttp();
