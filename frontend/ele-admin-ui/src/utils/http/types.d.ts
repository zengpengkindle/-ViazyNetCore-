import Axios, {
  Method,
  AxiosError,
  AxiosResponse,
  AxiosRequestConfig,
  InternalAxiosRequestConfig
} from "axios";

export type resultType = {
  accessToken?: string;
};

export type RequestMethods = Extract<
  Method,
  "get" | "post" | "put" | "delete" | "patch" | "option" | "head"
>;

export interface ViazyHttpError extends AxiosError {
  isCancelRequest?: boolean;
}

export interface ViazyHttpResponse extends AxiosResponse {
  config: ViazyHttpRequestConfig;
}

export interface ViazyHttpRequestConfig<D = any> extends InternalAxiosRequestConfig<D> {
  beforeRequestCallback?: (request: ViazyHttpRequestConfig) => void;
  beforeResponseCallback?: (response: ViazyHttpResponse) => any;
  beforeResponseErrorCallback?:(error:ViazyHttpError)=>void;
}

export default class ViazyHttp {
  request<T>(
    method: RequestMethods,
    url: string,
    param?: AxiosRequestConfig,
    axiosConfig?: ViazyHttpRequestConfig
  ): Promise<T>;
  post<T, P>(
    url: string,
    params?: T,
    config?: ViazyHttpRequestConfig
  ): Promise<P>;
  get<T, P>(
    url: string,
    params?: T,
    config?: ViazyHttpRequestConfig
  ): Promise<P>;
}
