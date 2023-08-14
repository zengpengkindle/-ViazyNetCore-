import { http } from "@/utils/http";
import { ComStatus } from "./model";
/**
 * FormTemplateAddRequest
 */
export interface FormTemplateEditRequest {
  id: number | null;
  /** 表单名称 */ name: string | null;
  /** 描述 */ description: string | null;
  formType: FormType | null;
}
/**
 * FormTemplateEditResult
 */
export interface FormTemplateEditResult {
  id: number;
  /** 表单名称 */ name: string;
  /** 描述 */ description: string;
  formType: FormType;
}
//
export enum FormType {
  /** 表单 */ Form = 1,
  /** 流程 */ Flow = 2,
  /** 问卷 */ Question = 3
}
/**
 * FormTemplateQueryRequest
 */
export interface FormTemplateQueryRequest {
  nameLike: string | null;
  status: ComStatus | null;
  formType: FormType | null;
  page: number | null;
  limit: number | null;
}

/**
 * FormTemplateResultPageData
 */
export interface FormTemplateResultPageData {
  rows: Array<FormTemplateResult>;
  total: number;
}

/**
 * FormTemplateResult
 */
export interface FormTemplateResult {
  id: number;
  /** 表单名称 */ name: string;
  /** 描述 */ description: string;
  sourceType: FormSourceType;
  /** 来源Id */ sourceId: number;
  publichStatus: FormStatus;
  status: ComStatus;
  formType: FormType;
}

//
export enum FormSourceType {
  /** 默认 */ Default = 0,
  /** 模板 */ Template = 2
}
//
export enum FormStatus {
  /** 未发布 */ Create = 1,
  /** 收集中 */ Release = 2,
  /** 停止发布 */ Stop = 3
}

export class FormTemplateApi {
  /**
   * 无
   */
  public edit(param1?: FormTemplateEditRequest): Promise<void> {
    return http.request({
      url: "api/formTemplate/edit",
      method: "post",
      data: param1
    });
  }
  /**F
   * 无
   */
  public list(
    param1?: FormTemplateQueryRequest
  ): Promise<FormTemplateResultPageData> {
    return http.request({
      url: "api/formTemplate/list",
      method: "post",
      data: param1
    });
  }
  /**
   * 无
   */
  public getInfo(id?: number): Promise<FormTemplateEditResult> {
    return http.request({
      url: "/api/formTemplate/getInfo",
      method: "get",
      params: { id }
    });
  }
  /**
   * 无
   */
  public saveFields(
    formId?: number,
    param1?: Array<FormWidgetResult>
  ): Promise<void> {
    return http.request({
      url: "/api/formTemplate/saveFields",
      method: "post",
      data: param1,
      params: { formId }
    });
  }
  /**
   * 无
   */
  public getFields(id?: number): Promise<Array<FormWidgetResult>> {
    return http.request({
      url: "/api/formTemplate/getFields",
      method: "get",
      params: { id }
    });
  }
}

/**
 * FormWidgetResult
 */
export interface FormWidgetResult {
  id: string | null;
  key: number | null;
  type: string | null;
  icon: string | null;
  formItemFlag: boolean | null;
  options: FormWidgetOptionResult | null;
}

/**
 * FormWidgetOptionResult
 */
export interface FormWidgetOptionResult {
  name: string;
  lable: string;
  labelAlign: string;
  type: string;
  defaultValue: string;
  placeholder: string;
  columnWidth: string;
  size: string;
  labelWidth: string;
  labelHidden: boolean;
  readonly: boolean;
  disabled: boolean;
  hidden: boolean;
  clearable: boolean;
  showPassword: boolean;
  required: boolean;
  requiredHint: string;
  validation: string;
  validationHint: string;
  showWordLimit: boolean;
  appendButton: boolean;
  appendButtonDisabled: boolean;
}

export default new FormTemplateApi();
