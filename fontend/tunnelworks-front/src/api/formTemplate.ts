/**
 * FormTemplateAddRequest
 */
export interface FormTemplateAddRequest {
  /** 表单名称 */ name: string | null;
  /** 描述 */ description: string | null;
  formType: FormType | null;
}

//
export enum FormType {
  /** 表单 */ Form = 1,
  /** 流程 */ Flow = 2,
  /** 问卷 */ Question = 3
}
import { http } from "@/utils/http";

export class FormTemplateApi {
  /**
   * 无
   */
  public add(param1?: FormTemplateAddRequest): Promise<void> {
    return http.request({
      url: "/formTemplate/add",
      method: "post",
      data: param1
    });
  }
}
export default new FormTemplateApi();
