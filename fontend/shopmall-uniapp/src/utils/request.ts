import { useToken } from "@/hooks/token";
import { useCommonStore } from "@/store/common";
import { stringify } from "qs";

const { token, clearToken } = useToken();

type AbortSignalEventType = "abort";
type AbortSignalEvent = {
  reason: string | Error;
  timeStamp: number;
};
type AbortSignalEventHandler = (
  this: RequestAbortSignal,
  ev: AbortSignalEvent
) => any;

class RequestAbortSignal {
  aborted: boolean;
  reason: string | Error;
  private eventHandlerList = new Map<
    AbortSignalEventType,
    Set<AbortSignalEventHandler>
  >();

  constructor() {
    this.aborted = false;
    this.reason = "";
  }
  addEventListener(
    type: AbortSignalEventType,
    listener: AbortSignalEventHandler
  ) {
    if (!this.eventHandlerList.get(type)) {
      this.eventHandlerList.set(type, new Set<AbortSignalEventHandler>());
    }
    this.eventHandlerList.get(type)!.add(listener);
  }
  removeEventListener(
    type: AbortSignalEventType,
    listener: AbortSignalEventHandler
  ) {
    this.eventHandlerList.get(type)!.delete(listener);
  }
  triggerEventListener(type: AbortSignalEventType) {
    this.eventHandlerList.get(type)!.forEach((listener) =>
      listener.call(this, {
        reason: this.reason,
        timeStamp: Date.now(),
      })
    );
  }
}

export class RequestAbortController {
  signal: RequestAbortSignal;

  constructor() {
    this.signal = new RequestAbortSignal();
  }

  abort(reason: string | Error) {
    this.signal.reason = reason;
    this.signal.triggerEventListener("abort");
  }
}

type RequestMethod = "get" | "post" | "put" | "head" | "options" | "delete";
export interface RequestInstance {
  abort: () => void;
}
export type AbortCallback = () => void;

export interface RequestOptionsBase {
  baseUrl?: string;
  url: string;
  method: RequestMethod;
  params?: any;
  data?: any;
}
export interface RequestOptionsEx {
  /**
   * 请求头
   */
  header?: any;
  /**
   * 请求超时事间
   * @default 30000
   */
  timeout?: number;
  /**
   * 终止控制信号，由{@link RequestAbortController}实例触发
   */
  signal?: RequestAbortSignal;
  /**
   * 第二种终止控制器，对于基础url相同的每个请求任务都会调用一次
   */
  controller?: (options: {
    params?: any;
    data?: any;
    req: RequestInstance;
  }) => void;
  /**
   * 终止回调，对于每个终止的请求任务都会调用一次回调函数
   */
  abortCallback?: AbortCallback;
  /**
   * 是否在请求出错时不弹出消息弹窗
   * @default false
   */
  silent?: boolean;
}
export interface RequestOptions extends RequestOptionsBase, RequestOptionsEx {}

const createUniqueKey = (
  method: string,
  url: string,
  params?: any,
  data?: any
) =>
  `${method}|${url}|${JSON.stringify(params || "")}|${JSON.stringify(
    data || ""
  )}`;

/**
 * 请求拦截器
 */
function requestIntercept(config: RequestOptions): RequestOptions {
  const common = useCommonStore();
  const url =
    common.env === "release"
      ? import.meta.env.VITE_FORMAL_BASE_URL
      : import.meta.env.VITE_BASE_URL;
  config.baseUrl = config.baseUrl || url;
  config.header = {
    "Content-Type": "application/json",
    Authorization: `Bearer ${token.value}`,
    ...config.header,
  };

  return config;
}

/**
 * @todo
 * 后处理数据
 */
function processResponse<T>(res: any): T {
  const data = res.data;
  const unwrappedData = data.result || data;
  return unwrappedData as T;
}

/**
 * 响应拦截器
 */
function responseIntercept<T>(
  res: UniApp.RequestSuccessCallbackResult,
  options: RequestOptionsEx
): Promise<T> {
  return new Promise<T>((resolve, reject) => {
    if (/^2\d{2}$/.test(res.statusCode.toString())) {
      // 2xx
      resolve(processResponse<T>(res.data));
    } else if (res.statusCode === 401 || res.statusCode === 409) {
      uni.redirectTo({ url: "/pages/authorization/index" });
      clearToken();
      uni.showToast({
        title: "登录过期，请重新登录~",
        icon: "none",
        duration: 2000,
      });
      reject(res);
    } else {
      const r = res.data as any;
      if (!options.silent && r?.data?.message) {
        uni.showToast({
          title: r.data.message,
          icon: "none",
        });
      }

      reject(r?.data?.message);
    }
  });
}

/**
 * 合并接口基础路径和路径参数
 */
function mergeUrl(url: string, params?: any): string {
  if (!params) {
    return url;
  } else if (url.includes("?")) {
    console.warn(
      `[Request] 请将查询参数写在\`params\`参数中而不是\`url\`中! url: ${url}`
    );
    return `${url}&${stringify(params)}`;
  } else {
    return `${url}?${stringify(params)}`;
  }
}

export interface WrappedRequestTask {
  params: any;
  data: any;
  task: UniApp.RequestTask;
}

const requestMap = new Map<string, WrappedRequestTask>();

/**
 * 请求函数
 */
export function request<R = any>(options: RequestOptions) {
  return new Promise<R>((resolve, reject) => {
    options = requestIntercept(options);
    const uniqueKey = createUniqueKey(
      options.method,
      options.url,
      options.data
    );

    if (options.controller) {
      const prefixUrl = `${options.method}|${options.url}`;
      for (const key of requestMap.keys()) {
        if (key.startsWith(prefixUrl)) {
          const val = requestMap.get(key)!;
          options.controller({
            params: val.params,
            data: val.data,
            req: val.task,
          });
        }
      }
    }

    const req = uni.request({
      url: options.baseUrl! + mergeUrl(options.url, options.params),
      method: options.method as any,
      header: options.header,
      data: options.data,
      timeout: options.timeout || 30000,
      success(res) {
        resolve(responseIntercept<R>(res, options));
      },
      fail(reason) {
        reject(reason);
      },
      complete() {
        requestMap.delete(uniqueKey);
        if (options.signal) {
          options.signal.removeEventListener("abort", abortHandler);
        }
      },
    });

    requestMap.get(uniqueKey)?.task.abort();
    requestMap.set(uniqueKey, {
      data: options.data,
      params: options.params,
      task: req,
    });

    const abortHandler = () => {
      req.abort();
      if (options.abortCallback) {
        options.abortCallback();
      }
    };

    if (options.signal) {
      options.signal.addEventListener("abort", abortHandler);
    }
  });
}
export const http = request; // 兼容写法
